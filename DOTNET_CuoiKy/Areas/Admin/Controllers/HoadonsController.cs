using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOTNET_CuoiKy.Models.DB;

namespace DOTNET_CuoiKy.Areas.admin.Controllers
{
    [Area("admin")]
    public class HoadonsController : Controller
    {
        private readonly comdatabaseContext _context;

        public HoadonsController(comdatabaseContext context)
        {
            _context = context;
        }

        // GET: admin/Hoadons
        public async Task<IActionResult> Index()
        {
            var comdatabaseContext = _context.Hoadon.Include(h => h.IdNguoimuaNavigation);
            return View(await comdatabaseContext.ToListAsync());
        }

        // GET: admin/Hoadons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadon
                .Include(h => h.IdNguoimuaNavigation)
                .FirstOrDefaultAsync(m => m.Idhoadon == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            return View(hoadon);
        }

        // GET: admin/Hoadons/Create
        public IActionResult Create()
        {
            ViewData["IdNguoimua"] = new SelectList(_context.Khachhang, "IdKhachHang", "Email");
            return View();
        }

        // POST: admin/Hoadons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idhoadon,IdNguoimua,IdchiTiet,TongTien,SoLuong,NgayTao,TinhTrang")] Hoadon hoadon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoadon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNguoimua"] = new SelectList(_context.Khachhang, "IdKhachHang", "Email", hoadon.IdNguoimua);
            return View(hoadon);
        }

        // GET: admin/Hoadons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadon.FindAsync(id);
            if (hoadon == null)
            {
                return NotFound();
            }
            ViewData["IdNguoimua"] = new SelectList(_context.Khachhang, "IdKhachHang", "Email", hoadon.IdNguoimua);
            return View(hoadon);
        }

        // POST: admin/Hoadons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idhoadon,IdNguoimua,IdchiTiet,TongTien,SoLuong,NgayTao,TinhTrang")] Hoadon hoadon)
        {
            if (id != hoadon.Idhoadon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoadon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoadonExists(hoadon.Idhoadon))
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
            ViewData["IdNguoimua"] = new SelectList(_context.Khachhang, "IdKhachHang", "Email", hoadon.IdNguoimua);
            return View(hoadon);
        }

        // GET: admin/Hoadons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadon
                .Include(h => h.IdNguoimuaNavigation)
                .FirstOrDefaultAsync(m => m.Idhoadon == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            return View(hoadon);
        }

        // POST: admin/Hoadons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoadon = await _context.Hoadon.FindAsync(id);
            _context.Hoadon.Remove(hoadon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoadonExists(int id)
        {
            return _context.Hoadon.Any(e => e.Idhoadon == id);
        }
    }
}
