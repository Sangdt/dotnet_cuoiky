using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DOTNET_CuoiKy.Helper;
using DOTNET_CuoiKy.Models.Client;
using DOTNET_CuoiKy.Models.DB;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_CuoiKy.Controllers
{
    public class CartController : Controller
    {
        comdatabaseContext _context;
        private string ShoppingCartId { get; set; }
        private const string CartSessionKey = "CartId";

        public CartController(comdatabaseContext context)
        {
            _context = context;
            //cart = new ShoppingCart(this.HttpContext, _context);
        }


        public IActionResult Index()
        {
           return View(GetCartitems());
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddtoCart([FromBody]addCartView model)
        {
            //var cart = ShoppingCart.GetCart(this.HttpContext);
            
            if (Additems(model))
            {
                return Ok("Đã thêm sản phẩm vào giỏ hàng cho bạn");
            }
            return NotFound("We fucked up");
        }


        private bool Additems(addCartView addInfo)
        {
            ShoppingCartId = GetCartId();
            var sptoAdd = _context.Sanpham.FirstOrDefault(sp => sp.IdsanPham == addInfo.id);

            if (sptoAdd != null)
            {
                // user already logged in and the cart migrations is gud
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var items = _context.Carts.SingleOrDefault(item => item.SpId == sptoAdd.IdsanPham && item.CartId.Equals(ShoppingCartId));
                    if (items == null)
                    {
                        //Guid random = new Guid();

                        items = new Carts
                        {
                            AutoId = Guid.NewGuid().ToString(),
                            SpId = sptoAdd.IdsanPham,
                            CartId = ShoppingCartId,
                            Quantity = addInfo.quantity,
                        };
                        _context.Carts.Add(items);
                    }
                    else
                    {
                        items.Quantity = items.Quantity + addInfo.quantity;
                    }
                    _context.SaveChanges();
                }
                else
                {
                    List<Carts> cartItems = HttpContext.Session.GetObjectFromJson<List<Carts>>(GetCartId()); ;
                    if (cartItems != null)
                    {
                        var items = cartItems.FirstOrDefault(item => item.SpId == sptoAdd.IdsanPham && item.CartId.Equals(ShoppingCartId));
                        if (items == null)
                        {
                            //Guid random = new Guid();
                            items = new Carts
                            {
                                AutoId = Guid.NewGuid().ToString(),
                                SpId = sptoAdd.IdsanPham,
                                CartId = ShoppingCartId,
                                Quantity = addInfo.quantity,
                                Sp = sptoAdd
                            };
                            cartItems.Add(items);
                        }
                        else
                        {
                            foreach (Carts item in cartItems)
                            {
                                if (item.CartId.Equals(items.CartId) && item.SpId == items.SpId)
                                {
                                    item.Quantity = item.Quantity + addInfo.quantity;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Guid random = new Guid();
                        cartItems = new List<Carts>
                        {
                            new Carts
                            {
                                AutoId = Guid.NewGuid().ToString(),
                                SpId = sptoAdd.IdsanPham,
                                CartId = ShoppingCartId,
                                Quantity = addInfo.quantity,
                                Sp =sptoAdd,
                            }
                        };
                    }
                    HttpContext.Session.SetObjectAsJson(GetCartId(), cartItems);
                }
                return true;
            }
            return false;
        }

        private List<Carts> GetCartitems()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return _context.Carts.Where(c => c.CartId.Equals(ShoppingCartId)).Include(sp => sp.Sp).ToList();
            }
            var cartItems = HttpContext.Session.GetObjectFromJson<List<Carts>>(GetCartId());
            return cartItems;
        }

        private string GetCartId()
        {
            if (HttpContext.Session.GetString(CartSessionKey) == null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    HttpContext.Session.SetString(CartSessionKey, HttpContext.User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Sid).Value);
                }
                else
                {
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            else if (HttpContext.User.Identity.IsAuthenticated && HttpContext.Session.GetString(CartSessionKey) != null)
            {
                HttpContext.Session.SetString(CartSessionKey, HttpContext.User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Sid).Value);
            }
            return HttpContext.Session.GetString(CartSessionKey);
        }

        //not sure why we need this but ok
        private string CartIdgenerator()
        {
            Random RandNum = new Random();
            string id = "";
            bool run = true;
            do
            {
                Guid random = new Guid();
                id = random.ToString();
                if (_context.Carts.Where(c => c.AutoId.Equals(id)) == null) run = false;
            } while (run);

            return id;
        }
    }
}