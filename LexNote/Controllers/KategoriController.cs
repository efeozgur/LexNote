using LexNote.Context;
using LexNote.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexNote.Controllers
{
    public class KategoriController : Controller
    {
        private readonly MyContext _context;

        public KategoriController(MyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kategoriler = await _context.Kategoriler.ToListAsync();

            return View(kategoriler);
        }

        public async Task<IActionResult> Ekle()
        {
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                await _context.Kategoriler.AddAsync(kategori);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(kategori);
        }

        public async Task<IActionResult> Duzenle(int id)
        {
            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }
            return View(kategori);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Kategori kategori)
        {
            if (id != kategori.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategori);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Kategoriler.Any(k => k.Id == kategori.Id))
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

            return View(kategori);
        }

        [HttpGet]
        public async Task<IActionResult> Sil(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategoriler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategori == null)
            {
                return NotFound();
            }

            return View(kategori);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }

            _context.Kategoriler.Remove(kategori);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}