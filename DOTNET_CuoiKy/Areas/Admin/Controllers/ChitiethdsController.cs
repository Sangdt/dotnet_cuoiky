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
    public class ChitiethdsController : Controller
    {
        private readonly comdatabaseContext _context;

        public ChitiethdsController(comdatabaseContext context)
        {
            _context = context;
        }

        // GET: admin/Chitiethds
        public async Task<IActionResult> Index()
        {
            var comdatabaseContext = _context.Chitiethd.Include(c => c.IdHdNavigation).Include(c => c.IdSpNavigation);
            return View(await comdatabaseContext.ToListAsync());
        }

        // GET: admin/Chitiethds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chitiethd = await _context.Chitiethd
                .Include(c => c.IdHdNavigation)
                .Include(c => c.IdSpNavigation)
                .FirstOrDefaultAsync(m => m.IdHd == id);
            if (chitiethd == null)
            {
                return NotFound();
            }

            return View(chitiethd);
        }

        // GET: admin/Chitiethds/Create
        public IActionResult Create()
        {
            ViewData["IdHd"] = new SelectList(_context.Hoadon, "Idhoadon", "Idhoadon");
            ViewData["IdSp"] = new SelectList(_context.Sanpham, "IdsanPham", "IdsanPham");
            return View();
        }

        // POST: admin/Chitiethds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHd,IdSp,Soluong,GiaSp")] Chitiethd chitiethd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chitiethd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdHd"] = new SelectList(_context.Hoadon, "Idhoadon", "Idhoadon", chitiethd.IdHd);
            ViewData["IdSp"] = new SelectList(_context.Sanpham, "IdsanPham", "IdsanPham", chitiethd.IdSp);
            return View(chitiethd);
        }

        // GET: admin/Chitiethds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chitiethd = await _context.Chitiethd.FindAsync(id);
            if (chitiethd == null)
            {
                return NotFound();
            }
            ViewData["IdHd"] = new SelectList(_context.Hoadon, "Idhoadon", "Idhoadon", chitiethd.IdHd);
            ViewData["IdSp"] = new SelectList(_context.Sanpham, "IdsanPham", "IdsanPham", chitiethd.IdSp);
            return View(chitiethd);
        }

        // POST: admin/Chitiethds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHd,IdSp,Soluong,GiaSp")] Chitiethd chitiethd)
        {
            if (id != chitiethd.IdHd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chitiethd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChitiethdExists(chitiethd.IdHd))
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
            ViewData["IdHd"] = new SelectList(_context.Hoadon, "Idhoadon", "Idhoadon", chitiethd.IdHd);
            ViewData["IdSp"] = new SelectList(_context.Sanpham, "IdsanPham", "IdsanPham", chitiethd.IdSp);
            return View(chitiethd);
        }

        // GET: admin/Chitiethds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chitiethd = await _context.Chitiethd
                .Include(c => c.IdHdNavigation)
                .Include(c => c.IdSpNavigation)
                .FirstOrDefaultAsync(m => m.IdHd == id);
            if (chitiethd == null)
            {
                return NotFound();
            }

            return View(chitiethd);
        }

        // POST: admin/Chitiethds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chitiethd = await _context.Chitiethd.FindAsync(id);
            _context.Chitiethd.Remove(chitiethd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChitiethdExists(int id)
        {
            return _context.Chitiethd.Any(e => e.IdHd == id);
        }
    }
}
