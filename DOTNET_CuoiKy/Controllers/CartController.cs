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
using SmartBreadcrumbs.Attributes;

namespace DOTNET_CuoiKy.Controllers
{
    public class CartController : Controller
    {
        comdatabaseContext _context;
        private static string ShoppingCartId { get; set; }
        private const string CartSessionKey = "CartId";

        public CartController(comdatabaseContext context)
        {
            _context = context;
            //cart = new ShoppingCart(this.HttpContext, _context);
        }

        [Breadcrumb("Giỏ hàng")]
        public IActionResult Index()
        {
            var ssID = HttpContext.Session.GetString(CartSessionKey);
            ShoppingCartId = ssID != null ? ssID : GetCartId();
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

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteItems(string itemID)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                var cartItems = HttpContext.Session.GetObjectFromJson<List<Carts>>(GetCartId());
                if (cartItems != null)
                {
                    var itemToRM = cartItems.FirstOrDefault(items => items.AutoId.Equals(itemID));
                    if (itemToRM != null)
                    {
                        cartItems.Remove(itemToRM);
                        HttpContext.Session.SetObjectAsJson(GetCartId(), cartItems);
                        return Ok("Đã xóa sản phẩm ra khỏi giỏ rồi nha");

                    }
                    return NotFound("Bro wtf did just send ?");
                }
                else
                {
                    return NotFound("Bro your cart is empty wtf ?");
                }
            }
            else
            {
                var cartItems = _context.Carts.ToList(); ;
                if (cartItems.Count() <= 0)
                {
                    return NotFound("Bro your cart is empty wtf ?");
                }
                else
                {
                    var itemToRM = cartItems.FirstOrDefault(items => items.AutoId.Equals(itemID));

                    if (itemToRM != null)
                    {
                        _context.Carts.Remove(itemToRM);
                        _context.SaveChanges();
                        return Ok("Đã xóa sản phẩm ra khỏi giỏ rồi nha");
                    }
                }
            }
            return NotFound("Bro wtf did just send ?");
        }

        [HttpPost, ActionName("Update")]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateItems([FromBody]UpdateCartView model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                var cartItems = HttpContext.Session.GetObjectFromJson<List<Carts>>(GetCartId());
                if (cartItems != null)
                {
                    //var itemToRM = cartItems.FirstOrDefault(items => items.AutoId.Equals(model.id));

                    var itemIndex = cartItems.FindIndex(sp => sp.AutoId.Equals(model.id.Trim()) && sp.CartId.Equals(ShoppingCartId));
                    cartItems[itemIndex].Quantity = model.quantity;
                    HttpContext.Session.SetObjectAsJson(GetCartId(), cartItems);
                    return Ok("Cap nhat r nha bo");
                }
            }
            else
            {
                var items = _context.Carts.FirstOrDefault(sp => sp.AutoId.Equals(model.id)&& sp.CartId.Equals(GetCartId()));
                if (items != null)
                {
                    items.Quantity = model.quantity;
                    _context.Carts.Update(items);
                    _context.SaveChangesAsync();
                    return Ok("Cap nhat r nha bo");
                }
            }
            return NotFound("Broo what are u doingg");
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
                    var items = _context.Carts
                        .SingleOrDefault(item => item.SpId == sptoAdd.IdsanPham && item.CartId.Equals(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value));
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
                return _context.Carts.Where(c => c.CartId.Equals(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)).Include(sp => sp.Sp).ToList();
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
                    HttpContext.Session.SetString(CartSessionKey, HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
                HttpContext.Session.SetString(CartSessionKey, HttpContext.User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.NameIdentifier).Value);
            }
            return HttpContext.Session.GetString(CartSessionKey);
        }

        //not sure why we need this but ok
        //private string CartIdgenerator()
        //{
        //    Random RandNum = new Random();
        //    string id = "";
        //    bool run = true;
        //    do
        //    {
        //        Guid random = new Guid();
        //        id = random.ToString();
        //        if (_context.Carts.Where(c => c.AutoId.Equals(id)) == null) run = false;
        //    } while (run);

        //    return id;
        //}
    }
}