using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOTNET_CuoiKy.Models.DB;
using MySql.Data.MySqlClient;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace DOTNET_CuoiKy.Areas.admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class SanphamsController : Controller
    {
        private readonly comdatabaseContext _context;
       // private List<Sanpham> spLst=null;
        public SanphamsController(comdatabaseContext context)
        {
            _context = context;
        }
        private IQueryable<Sanpham> GetSanphams()
        {
            bool success = false;
            int retryCount = 0;
            MySqlException exception = null;
            while (!success && retryCount <= 6)
            {
                try
                {
                   var spLst = _context.Sanpham.Include(s => s.DanhMucNavigation);
                    success = true;
                    retryCount = 5;
                }
                catch (MySqlException e)
                {
                    exception = e;
                    success = false;
                    retryCount++;
                    Thread.Sleep(800);
                }
            }
            if (retryCount > 5)
            {
                throw exception;
            }
            return _context.Sanpham.Include(s => s.DanhMucNavigation);
        }

        // GET: admin/Sanphams
        public async Task<IActionResult> Index()
        {
            bool isAjaxCall = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            var comdatabaseContext = _context.Sanpham.Include(s => s.DanhMucNavigation);
            //var comdatabaseContext = GetSanphams();
            //if (isAjaxCall)
            //{
            //    //return Json(await comdatabaseContext.ToListAsync());
            //    return new JsonResult(await comdatabaseContext.ToListAsync());
            //}
            return View( await comdatabaseContext.ToListAsync());
        }

        // GET: admin/Sanphams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanpham
                .Include(s => s.DanhMucNavigation)
                .FirstOrDefaultAsync(m => m.IdsanPham == id);
            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // GET: admin/Sanphams/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Danhmuc, "IddanhMuc", "IddanhMuc");
            return View();
        }

        // POST: admin/Sanphams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdsanPham,TenSp,GiaSp,Hinh1,Hinh2,Hinh3,Hinh4,MoTa,DanhMuc")] Sanpham sanpham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanpham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Danhmuc, "IddanhMuc", "IddanhMuc", sanpham.DanhMuc);
            return View(sanpham);
        }

        // GET: admin/Sanphams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanpham.FindAsync(id);
            if (sanpham == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Danhmuc, "IddanhMuc", "IddanhMuc", sanpham.DanhMuc);
            return View(sanpham);
        }

        // POST: admin/Sanphams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdsanPham,TenSp,GiaSp,Hinh1,Hinh2,Hinh3,Hinh4,MoTa,DanhMuc")] Sanpham sanpham)
        {
            if (id != sanpham.IdsanPham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanpham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanphamExists(sanpham.IdsanPham))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Danhmuc, "IddanhMuc", "IddanhMuc", sanpham.DanhMuc);
            return View(sanpham);
        }

        // GET: admin/Sanphams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanpham
                .Include(s => s.DanhMucNavigation)
                .FirstOrDefaultAsync(m => m.IdsanPham == id);
            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // POST: admin/Sanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string itemId)
        {
            var sanpham = await _context.Sanpham.FindAsync(int.Parse(itemId));
            if (sanpham != null)
            {
                string tenSp = sanpham.TenSp;
                _context.Sanpham.Remove(sanpham);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                var newspLSt = _context.Sanpham.Include(dm => dm.DanhMucNavigation);
                return Ok("Xóa sản phẩm "+tenSp+ " được rồi nè dân chơi !!!");
            }
            return NotFound("Không xóa được rồi, kiếm không ra");
          
        }

        private bool SanphamExists(int id)
        {
            return _context.Sanpham.Any(e => e.IdsanPham == id);
        }
    }
}
