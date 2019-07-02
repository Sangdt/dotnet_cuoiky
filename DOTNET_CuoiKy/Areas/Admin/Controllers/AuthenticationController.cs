using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using DOTNET_CuoiKy.Models.DB;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using DOTNET_CuoiKy.Helper;

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
        private bool checkUserinfo(Admins user)
        {
            return _context.Admin.FirstOrDefault(n => n.Username.Equals(user.Username)) != null && _context.Admin.FirstOrDefault(n => n.Password.Equals(user.Password)) != null;
        }
        [AllowAnonymous]
        // GET: Authentication
        public ActionResult Index(string returnUrl)
        {
            if (UserStatus.getUserStatus(this,"admin"))
            {
                return RedirectToAction("Index", "Home", new { message = "Đăng nhập rồi bạn ie" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Admins admin, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (checkUserinfo(admin))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,admin.Username),
                        new Claim(ClaimTypes.Role,"Admin")
                    };
                    ClaimsIdentity user = new ClaimsIdentity(claims, "Admin");
                    ClaimsPrincipal principal = new ClaimsPrincipal(user);

                    await HttpContext.SignInAsync("Admin", principal);
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