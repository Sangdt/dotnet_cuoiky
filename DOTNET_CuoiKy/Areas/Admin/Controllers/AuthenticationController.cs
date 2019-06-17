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

    [Area("Admin")]
    public class AuthenticationController : Controller
    {
        private readonly comdatabaseContext _context;
        public AuthenticationController(comdatabaseContext context)
        {
            _context = context;
        }
        private bool checkUserinfo(Admin user)
        {
            return _context.Admin.FirstOrDefault(n => n.Username.Equals(user.Username)) != null && _context.Admin.FirstOrDefault(n => n.Password.Equals(user.Password)) != null;

         
        }
        [AllowAnonymous]
        // GET: Authentication
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { message = "Đăng nhập rồi bạn ie" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Admin admin, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var cc = checkUserinfo(admin);
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
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                    //return Redirect("/Admin/");
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