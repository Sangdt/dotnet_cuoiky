using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DOTNET_CuoiKy.Models.DB;
using DOTNET_CuoiKy.Models;
using SmartBreadcrumbs;
using SmartBreadcrumbs.Nodes;
using SmartBreadcrumbs.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_CuoiKy.Controllers
{
    public class HomeController : Controller
    {
        private readonly comdatabaseContext db;
        public HomeController(comdatabaseContext context)
        {
            db = context;
        }

        [DefaultBreadcrumb("Trang chủ")]
        public IActionResult Index(string message)
        {
            ViewData["LoginMess"] = message;
            return View(new List<Sanpham>());
        }

        [HttpGet("/danhmucs/{dmID}")]
        public IActionResult Danhmucs(int dmID)
        {
            var dm = db.Danhmuc.FirstOrDefault(n => n.IddanhMuc == dmID);
            if (dm != null)
            {
                List<Sanpham> spLst = db.Sanpham.Where(n => n.DanhMuc == dmID).ToList();
                var categoryNode = new MvcBreadcrumbNode("DanhMucs", "Home", dm.TenDm.ToUpper());
                ViewData["BreadcrumbNode"] = categoryNode;

                return View(spLst);
            }
            return RedirectToAction("Error");
        }

        [HttpGet("/sanphams/{idsp}")]
        public IActionResult Chitiets(int idsp)
        {
            var sanpham = db.Sanpham.Include(dm => dm.DanhMucNavigation).FirstOrDefault(n => n.IdsanPham == idsp);
            if (sanpham != null)
            {
                // Manually create the nodes (assuming you used the attribute to create a Default node, otherwise create it manually too).
                var categoryNode = new MvcBreadcrumbNode("DanhMucs", "Home", sanpham.DanhMucNavigation.TenDm.ToUpper())
                {
                    RouteValues = new { dmID = sanpham.DanhMuc.Value }
                };
                // When manually creating nodes, you have the option to use route values in case you need them.
                var productNode = new MvcBreadcrumbNode("Chitiets", "Home", sanpham.TenSp)
                {
                    Parent = categoryNode
                };

                //List<Sanpham> spLst = db.Sanpham.Where(n => n.IdsanPham == idsp).ToList();
                ViewData["BreadcrumbNode"] = productNode;
                return View(sanpham);
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

        public async Task<IActionResult> timkiem(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var food = db.Sanpham.Where(s => s.TenSp.Contains(searchString));
                if (food.Count() > 0)
                {
                    return View("Index", await food.ToListAsync());
                }
            }
            ViewData["TIMKIEM"] ="Nhap clgv ??? hong kiem thay cha";
            return View("Index", new List<Sanpham>());

        }
    }
}
