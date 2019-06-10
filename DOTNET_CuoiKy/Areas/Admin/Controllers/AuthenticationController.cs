using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOTNET_CuoiKy.Models.DB;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace DOTNET_CuoiKy.Areas.admin.Controllers
{
    //[Authorize(AuthenticationSchemes = "Admin")]

    [Area("admin")]
    public class AuthenticationController : Controller
    {
        private readonly comdatabaseContext _context;
        public AuthenticationController(comdatabaseContext context)
        {
            _context = context;
        }
        private bool checkUserinfo(Admin user)
        {
            if (_context.Admin.FirstOrDefault(n => n.Username.Equals(user.Username)) != null || _context.Admin.FirstOrDefault(n => n.Password.Equals(user.Password)) != null)
            {
                if (_context.Admin.FirstOrDefault(n => n.Password.Equals(user.Password)) != null)
                {
                    return true;
                }
            }
            return false;
        }
        [AllowAnonymous]
        // GET: Authentication
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Homeadmin", new { message = "Đăng nhập rồi bạn ie" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Admin admin)
        {
            if (ModelState.IsValid)
            {
                if (checkUserinfo(admin))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,admin.Username),
                        new Claim(ClaimTypes.Role,"admin")
                    };
                    ClaimsIdentity user = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(user);

                    await HttpContext.SignInAsync("Admin",principal);
                    return Redirect("/Admin/");
                }
                else
                {
                    ViewData["UserLoginFailed"] = "Sai tên đăng nhập hoặc mật khẩu r nha bạn ";
                }
            }
            return View();
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Admin");
            return RedirectToAction("Index");
        }
        // GET: Authentication/Details/5
    }
}