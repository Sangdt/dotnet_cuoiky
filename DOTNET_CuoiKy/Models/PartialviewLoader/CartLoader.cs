using DOTNET_CuoiKy.Helper;
using DOTNET_CuoiKy.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DOTNET_CuoiKy.Models.PartialviewLoader
{
    public class CartLoader : ViewComponent
    {
        private readonly comdatabaseContext _context;
        public CartLoader(comdatabaseContext context)
        {
            _context = context;
        }
        private List<Carts> GetCartitems()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return _context.Carts.Where(c => c.CartId.Equals(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)).Include(sp => sp.Sp).ToList();
            }
            var id = HttpContext.Session.GetString("CartId");
            var cartItems = id!=null? HttpContext.Session.GetObjectFromJson<List<Carts>>(id):null;
            return cartItems;
        }

        public IViewComponentResult Invoke()
        {
            List<Carts> dmLst = GetCartitems();

            return View("CartLoader", dmLst);
        }
    }
}
