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
using DOTNET_CuoiKy.Models;

namespace DOTNET_CuoiKy.Controllers
{
    public class LoginController : Controller
    {
        private readonly comdatabaseContext _context;

        public LoginController(comdatabaseContext context)
        {
            _context = context;
        }
        private Khachhang checkUserinfo(LoginRegisterModel model)
        {
            var kh = _context.Khachhang.FirstOrDefault(n => n.Email.Equals(model.userName) || n.NameKh.Equals(model.userName));
            if (kh!=null)
            {
                if(kh.Password.Equals(model.passWord))
                    return kh;
                else
                {
                    return null;
                }
            }
            return null;
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { message = "Đăng nhập rồi bạn ie" });
            }
            return View();
        }
        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRegisterModel Lmodel)
        {
            if (ModelState.IsValid)
            {
                var kh = checkUserinfo(Lmodel);
                if (kh!=null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.NameKh!=null? kh.NameKh : kh.Email),
                        new Claim(ClaimTypes.NameIdentifier, kh.IdKhachHang.ToString())
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
            string password2 = PasswordCrypt.CreateMD5(registerModel.passWord);
            if (ModelState.IsValid)
            {

                if (checkUserinfosignup(registerModel))
                {
                    Khachhang obj = new Khachhang();
                    obj.Email = registerModel.userName;
                    //obj.SoDiethoai = registerModel.userName;
                    obj.Password = password2;
                    _context.Khachhang.Add(obj);
                    _context.SaveChanges();
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