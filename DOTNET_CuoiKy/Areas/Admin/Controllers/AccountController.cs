using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using DOTNET_CuoiKy.Models.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace DOTNET_CuoiKy.Areas.admin.Controllers
{

    [Authorize(AuthenticationSchemes = "Admin")]
    [Area("admin")]
    public class AacountController : Controller
    {
        private readonly comdatabaseContext _context;
        public AacountController(comdatabaseContext context)
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

        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Homeadmin", new { message = "Đăng nhập rồi bạn ie" });
            }
            return View();
        }
        //public IActionResult Login(FormCollection collection)
        //{
        //    var tendn = collection["adacc"];
        //    var matkhau = collection["adpass"];
        //    if (ModelState.IsValid)
        //    {

        //        if (String.IsNullOrEmpty(tendn))
        //        {
        //            ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
        //        }
        //        else if (String.IsNullOrEmpty(matkhau))
        //        {
        //            ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
        //        }
        //        else
        //        {
        //            Models.DB.Admin ad = _context.Admin.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);
        //            if (ad != null)
        //            {
        //                ISession[]= tendn;
        //                return RedirectToAction("Index", "trangchu");
        //            }
        //            else
        //                ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
        //        }
        //    }

        //    return View();

        //}
       [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }


    }
}