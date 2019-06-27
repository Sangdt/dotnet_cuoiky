using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DOTNET_CuoiKy.Models.DB;
using DOTNET_CuoiKy.Models.Client;


namespace DOTNET_CuoiKy.Helper
{
    public class ShoppingCart
    {
        comdatabaseContext db = new comdatabaseContext() ;
        public string ShoppingCartId { get; set; }
        //private comdatabaseContext _context;
        public const string CartSessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContext context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        //public ShoppingCart(HttpContext context, comdatabaseContext DBcontext)
        //{
        //    ShoppingCartId = GetCartId(context);
        //    db = DBcontext;
        //}
        public bool Additems(HttpContext context, addCartView addInfo)
        {
            var sptoAdd = db.Sanpham.FirstOrDefault(sp => sp.IdsanPham == addInfo.id);

            if (sptoAdd != null)
            {
                // add cart goes here

                // user already logged in and the cart migrations is gud
                if (context.User.Identity.IsAuthenticated)
                {
                    var items = db.Carts.SingleOrDefault(item => item.SpId == sptoAdd.IdsanPham && item.CartId.Equals(ShoppingCartId));
                    if (items == null)
                    {
                        items = new Carts
                        {
                            AutoId = CartIdgenerator(),
                            SpId = sptoAdd.IdsanPham,
                            CartId = ShoppingCartId,
                            Quantity = addInfo.quantity,
                        };
                        db.Carts.Add(items);
                    }
                    else
                    {
                        items.Quantity = items.Quantity + addInfo.quantity;
                    }
                    db.SaveChanges();
                }
                else
                {
                    List<Carts> cartItems = SessionExtensions.GetObjectFromJson<List<Carts>>(context.Session, ShoppingCartId);
                    if (cartItems != null)
                    {
                        var items = cartItems.SingleOrDefault(item => item.SpId == sptoAdd.IdsanPham && item.CartId.Equals(ShoppingCartId));
                        if (items == null)
                        {
                            items = new Carts
                            {
                                AutoId = CartIdgenerator(),
                                SpId = sptoAdd.IdsanPham,
                                CartId = ShoppingCartId,
                                Quantity = addInfo.quantity,
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
                        Guid random = new Guid();
                        cartItems = new List<Carts>
                        {
                            new Carts
                            {
                                AutoId = random.ToString(),
                                SpId = sptoAdd.IdsanPham,
                                CartId = ShoppingCartId,
                                Quantity = addInfo.quantity,
                                Sp =sptoAdd,
                            }
                        };
                    }
                    SessionExtensions.SetObjectAsJson(context.Session, ShoppingCartId, cartItems);
                }
                return true;
            }
            return false;
        }

        public List<Carts> GetCartitems(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                return db.Carts.Where(c => c.CartId.Equals(ShoppingCartId)).Include(sp=>sp.Sp).ToList();
            }
            List<Carts> cartSS = SessionExtensions.GetObjectFromJson<List<Carts>>(context.Session, ShoppingCartId);
            return cartSS;
        }

        public string GetCartId(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Session.SetString(CartSessionKey, context.User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Sid).Value);
                }
                else
                {
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempCartId = Guid.NewGuid();
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            else if (context.User.Identity.IsAuthenticated && context.Session.GetString(CartSessionKey) != null)
            {
                context.Session.SetString(CartSessionKey, context.User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Sid).Value);
            }
            return context.Session.GetString(CartSessionKey);
        }

        private string CartIdgenerator()
        {
            Random RandNum = new Random();
            string id = "";
            bool run = true;
            do
            {
                Guid random = new Guid();
                id = random.ToString();
                if(db.Carts.Where(c => c.AutoId.Equals(id))==null) run = false;
            } while(run);

            return id;
        }
    }
}
