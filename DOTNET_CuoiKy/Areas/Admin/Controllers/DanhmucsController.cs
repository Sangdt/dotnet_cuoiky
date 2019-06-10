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
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "admin")]
    public class DanhmucsController : Controller
    {
        private readonly comdatabaseContext _context;

        public DanhmucsController(comdatabaseContext context)
        {
            _context = context;
        }

        // GET: admin/Danhmucs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Danhmuc.ToListAsync());
        }

        // GET: admin/Danhmucs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhmuc = await _context.Danhmuc
                .FirstOrDefaultAsync(m => m.IddanhMuc == id);
            if (danhmuc == null)
            {
                return NotFound();
            }

            return View(danhmuc);
        }

        // GET: admin/Danhmucs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Danhmucs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IddanhMuc,TenDm")] Danhmuc danhmuc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhmuc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(danhmuc);
        }

        // GET: admin/Danhmucs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhmuc = await _context.Danhmuc.FindAsync(id);
            if (danhmuc == null)
            {
                return NotFound();
            }
            return View(danhmuc);
        }

        // POST: admin/Danhmucs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IddanhMuc,TenDm")] Danhmuc danhmuc)
        {
            if (id != danhmuc.IddanhMuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhmuc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhmucExists(danhmuc.IddanhMuc))
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
            return View(danhmuc);
        }

        // GET: admin/Danhmucs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhmuc = await _context.Danhmuc
                .FirstOrDefaultAsync(m => m.IddanhMuc == id);
            if (danhmuc == null)
            {
                return NotFound();
            }

            return View(danhmuc);
        }

        // POST: admin/Danhmucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhmuc = await _context.Danhmuc.FindAsync(id);
            _context.Danhmuc.Remove(danhmuc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhmucExists(int id)
        {
            return _context.Danhmuc.Any(e => e.IddanhMuc == id);
        }
    }
}
