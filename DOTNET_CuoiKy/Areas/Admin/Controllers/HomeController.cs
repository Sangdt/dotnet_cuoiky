using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_CuoiKy.Areas.admin.Controllers
{

    [Authorize(AuthenticationSchemes = "Admin")]
    [Area("admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            return View();


            //if (ISession["accname"] == null)
            //{
            //    ISession["accname"] = null;
            //    return RedirectToAction("Login", "useradmin");
            //}
            //else
            //{
            //    
            //}

        }
    }
}