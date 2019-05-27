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
        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View();
        }

        private bool checkUser(LoginModel model)
        {
            if (_context.Khachhang.FirstOrDefault(n => n.Email.Equals(model.userName))!= null || _context.Khachhang.FirstOrDefault(n => n.SoDiethoai.Equals(model.userName))!=null)
            {
                if (_context.Khachhang.FirstOrDefault(n => n.Password.Equals(model.passWord))!=null)
                {
                    return true;
                }
            }
            return false;
        }
        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel Lmodel)
        {
            if (ModelState.IsValid)
            {
                if (checkUser(Lmodel))
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

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }
    }
}