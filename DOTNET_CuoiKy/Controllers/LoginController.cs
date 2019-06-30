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
using Microsoft.AspNetCore.Authorization;
using DOTNET_CuoiKy.Models;
using Microsoft.AspNetCore.Http;
using DOTNET_CuoiKy.Helper;
using SmartBreadcrumbs.Nodes;

namespace DOTNET_CuoiKy.Controllers
{
    public class LoginController : Controller
    {
        private readonly comdatabaseContext _context;
        private const string CartSessionKey = "CartId";

        public LoginController(comdatabaseContext context)
        {
            _context = context;
        }
        private Khachhang checkUserinfo(LoginRegisterModel model)
        {
            var kh = _context.Khachhang.FirstOrDefault(n => n.Email.Equals(model.userName) || n.NameKh.Equals(model.userName));
            if (kh!=null)
            {
                var pass = PasswordCrypt.CreateMD5(model.passWord);
                if(kh.Password.Equals(pass))
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
            if (UserStatus.getUserStatus(this, "Client"))
            {
                return RedirectToAction("Index", "Home", new { message = "Đăng nhập rồi bạn ie" });
            }
            var loginNode = new MvcBreadcrumbNode("Login", "Login", "Đăng nhập");
            ViewData["BreadcrumbNode"] = loginNode;
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
                        new Claim(ClaimTypes.Role, "Client"),
                        new Claim(ClaimTypes.NameIdentifier, kh.IdKhachHang.ToString())
                    };
                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    var oldCartID = HttpContext.Session.GetString(CartSessionKey);
                    var cartList = oldCartID!=null ? HttpContext.Session.GetObjectFromJson<List<Carts>>(oldCartID) : new List<Carts>();
                    List<Carts> lstToadd = new List<Carts>();
                    if (cartList != null&&cartList.Count()>0)
                    {
                        foreach(var item in cartList)
                        {
                            var add = true;
                            item.Sp = null;
                            item.CartId = kh.IdKhachHang.ToString();
                            var spExisted = _context.Carts.FirstOrDefault(sp => sp.SpId == item.SpId && sp.CartId.Equals(item.CartId));
                            // if user carts already exist we just want to update quanity instead adding new
                            // just exclude it out of the things to add
                            if (spExisted != null)
                            {
                                spExisted.Quantity += item.Quantity;
                                _context.Carts.Update(spExisted);
                                add = false;
                            }
                            if (add)
                            {
                                lstToadd.Add(item);
                            }
                        }
                        //make sure shit in session is clean af so we can go on with items from db
                        HttpContext.Session.SetObjectAsJson(oldCartID, "");
                        await _context.Carts.AddRangeAsync(lstToadd);
                    }

                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString(CartSessionKey, kh.IdKhachHang.ToString());

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return Redirect("/");
                }
                else
                {
                    ViewData["UserLoginFailed"] = "Sai tên đăng nhập hoặc mật khẩu r nha bạn ";
                }
            }
            return View();
        }

        //[Breadcrumb("Đăng ký tài khoản")]
        [HttpGet("/register")]
        public IActionResult Register()
        {
            var loginNode = new MvcBreadcrumbNode("Register", "Login", "Tạo tài khoản mới");
            ViewData["BreadcrumbNode"] = loginNode;
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
            HttpContext.Session.SetString(CartSessionKey,"");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}