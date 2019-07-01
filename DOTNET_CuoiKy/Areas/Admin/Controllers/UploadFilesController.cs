using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_CuoiKy.Areas.admin.Controllers
{
    public class UploadFilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}