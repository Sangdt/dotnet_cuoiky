using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DOTNET_CuoiKy.Models.DB;
using DOTNET_CuoiKy.Models.Client;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace DOTNET_CuoiKy.Controllers
{
    public class LoginController : Controller
    {
        private readonly comdatabaseContext _context;

        public LoginController(comdatabaseContext context)
        {
            _context = context;
        }
        private bool checkUserinfo(LoginRegisterModel model)
        {
            if (_context.Khachhang.FirstOrDefault(n => n.Email.Equals(model.userName)) != null || _context.Khachhang.FirstOrDefault(n => n.SoDiethoai.Equals(model.userName)) != null)
            {
                if (_context.Khachhang.FirstOrDefault(n => n.Password.Equals(model.passWord)) != null)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","Home", new { message = "Đăng nhập rồi bạn ie" });
            }
            return View();
        }
        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRegisterModel Lmodel)
        {
            if (ModelState.IsValid)
            {
                if (checkUserinfo(Lmodel))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,Lmodel.userName)
                    };
                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal);
                    return Redirect("/");
                }
                else
                {
                    ViewData["UserLoginFailed"] = "Sai tên đăng nhập hoặc mật khẩu r nha bạn ";
                }
            }
            return View();
        }

        
        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View();
        }
        private bool checkUserinfosignup(LoginRegisterModel model)
        {
            if (_context.Khachhang.FirstOrDefault(n => n.Email.Equals(model.userName)) != null || _context.Khachhang.FirstOrDefault(n => n.SoDiethoai.Equals(model.userName)) != null)
            {
               return false;
            }
            return true;
        }
        [HttpPost("/register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(LoginRegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (checkUserinfosignup(registerModel))
                {
                    return Redirect("/login");
                }
                else
                {
                    ViewData["UserLoginFailed"] = "Trùng tên đăng nhập rồi nha khứa ";
                }
            }
            return View();
        }
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }
    }
}