using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOTNET_CuoiKy.Models.DB;
using Microsoft.AspNetCore.Authorization;

namespace DOTNET_CuoiKy.Areas.admin.Controllers
{

    [Authorize(AuthenticationSchemes = "Admin")]
    [Area("admin")]
    public class SanphamsController : Controller
    {
        private readonly comdatabaseContext _context;

        public SanphamsController(comdatabaseContext context)
        {
            _context = context;
        }

        // GET: admin/Sanphams
        public async Task<IActionResult> Index()
        {
            var comdatabaseContext = _context.Sanpham.Include(s => s.DanhMucNavigation);
            return View(await comdatabaseContext.ToListAsync());
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanpham = await _context.Sanpham.FindAsync(id);
            _context.Sanpham.Remove(sanpham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanphamExists(int id)
        {
            return _context.Sanpham.Any(e => e.IdsanPham == id);
        }
    }
}
