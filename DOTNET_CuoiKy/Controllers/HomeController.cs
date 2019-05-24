using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DOTNET_CuoiKy.Models;

namespace DOTNET_CuoiKy.Controllers
{
    public class HomeController : Controller
    {
        private readonly comdbContext db;
        public HomeController (comdbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("/sanphams/{idsp}")]
        public IActionResult Chitiets(int idsp)
        {
            if (db.Sanpham.FirstOrDefault(n => n.IdsanPham == idsp) != null)
            {
                List<Sanpham> spLst = db.Sanpham.Where(n => n.IdsanPham == idsp).ToList();
                return View(spLst);
            }
            return RedirectToAction("Error");
        }
        [HttpGet("/danhmucs/{dmID}")]
        public IActionResult Danhmucs(int dmID)
        {
            if(db.Danhmuc.FirstOrDefault(n => n.IddanhMuc == dmID) != null)
            {
                List<Sanpham> spLst = db.Sanpham.Where(n => n.DanhMuc == dmID).ToList();
                return View(spLst);
            }
            return RedirectToAction("Error");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
