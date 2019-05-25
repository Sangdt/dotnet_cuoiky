using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using DOTNET_CuoiKy.Models;
namespace DOTNET_CuoiKy.Controllers
{
    [Route("Khachhang")]
    public class userController : Controller
    {
        
        private readonly comdbContext db;

        public userController(comdbContext context)
        {
            db = context;
        }
        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("signup")]
        public IActionResult SignUp()
        {
            return View("SignUp",new Khachhang());
        }

        //public bool checkUser(int name, string pass)
        //{
        //    if (db.Khachhang.Where(m => m.SoDiethoai == name && m.Password == pass).Count() > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //public IActionResult LogIn()
        //{
        //    return View();
        //}
        //public IActionResult Register()
        //{
        //    return View();  
        //}
        //[HttpPost]
        //public IActionResult Register(Khachhang user)
        //{
        //    string password2 = PasswordCrypt.CreateMD5(user.Password);
        //    //xét email ko trùng
        //    if (ModelState.IsValid && db.Khachhang.Where(m => m.SoDiethoai.Equals(user.SoDiethoai)).Count() == 0)

        //    {
        //        Khachhang objKH = new Khachhang();
        //        objKH.NameKh = user.NameKh;
        //        objKH.Email = user.Email;
        //        objKH.Password = password2;
        //        objKH.SoDiethoai = user.SoDiethoai;
        //        objKH.Address = user.Address;



        //        //chèn dữ liệu vào bảng khách hàng
        //        db.Khachhang.Add(objKH);
        //        //lưu vào csdl

        //        db.SaveChanges();

        //        return RedirectToAction("Login", "user" );
        //    }
        //    else if (ModelState.IsValid && db.Khachhang.Where(m => m.SoDiethoai.Equals(user.SoDiethoai)).Count() > 0)
        //    // trùng email
        //    {
        //        ModelState.AddModelError("Email", "Email đã tồn tại !");
        //    }
        //    return View("Register");
        //}
        ////public IActionResult LogOff()
        ////{
        ////    //FormsAuthentication.SignOut(); //xoa cookie

        ////    //WebSecurity.Logout();
        ////    Session["Name"] = null;
        ////    return RedirectToAction("LogIn", "User");
        ////}
        //[HttpPost]
        //public IActionResult LogIn(Khachhang user1)
        //{
        //    //dùng để clear lỗi của ràng buộc nhập tên kh.
        //    //ModelState.Where(m => m.Key == "tenkh").FirstOrDefault().Value.Errors.Clear();
        //    //ModelState.Where(m => m.Key == "sdt").FirstOrDefault().Value.Errors.Clear();
        //    if (ModelState.IsValid)
        //    {
        //        string pass = PasswordCrypt.CreateMD5(user1.Password);
        //        //ModelState.Where(m => m.Key == "tenkh").FirstOrDefault().Value.Errors.Clear();
        //        if (db.Khachhang.Where(m => m.SoDiethoai == user1.SoDiethoai && m.Password == pass).Count() == 1)

        //        {
        //            Khachhang kh = db.Khachhang.Where(m => m.SoDiethoai == user1.SoDiethoai && m.Password == pass).FirstOrDefault();
        //            //gọi hàm GetRolesForUser(string username) 
        //            //trong fie CustomRoleProvider.cs) 
        //            //nó sẽ chuyền giá trị username để lấy quyền.
        //            //FormsAuthentication.SetAuthCookie(user1.Email, true);

        //            //Session["Name"] = user1.SoDiethoai;
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else if (db.Khachhang.Where(m => m.SoDiethoai == user1.SoDiethoai && m.Password == pass).Count() == 0)
        //        {
        //            ViewBag.ErrorMessage = "SAI TÊN ĐĂNG NHẬP HOẶC MẬT KHẨU\n Chưa có tài khoản? Bấm Đăng ký";
        //        }
        //    }
        //    return View(user1);

        //}

    }
}