using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_CuoiKy.Areas.admin.Controllers
{
  

    [Area("admin")]
    public class HomeController : Controller
    {
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}