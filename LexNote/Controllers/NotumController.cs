using LexNote.Context;
using LexNote.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LexNote.Controllers
{
    public class NotumController : Controller
    {
        private readonly MyContext _context;

        public NotumController(MyContext context)
        {
            _context = context;
        }

        private void KategorileriYukle()
        {
            ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "Ad");
        }

        public IActionResult Index(string? q, string? etiket)
        {
            var notlar = _context.Notlar.Include(x => x.Kategori).ToList();

            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();
                notlar = notlar
                    .Where(n => (n.Baslik?.ToLower().Contains(q) ?? false) ||
                                (n.Icerik?.ToLower().Contains(q) ?? false) ||
                                (n.Etiketler?.ToLower().Contains(q) ?? false))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(etiket))
            {
                notlar = notlar
                    .Where(n => n.Etiketler != null &&
                                n.Etiketler.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(e => e.Trim().ToLower())
                                .Contains(etiket.ToLower()))
                    .ToList();
            }

            ViewBag.Arama = q;
            ViewBag.Etiket = etiket;

            return View(notlar);
        }

        public IActionResult Ekle()
        {
            KategorileriYukle();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Notum not)
        {
            if (ModelState.IsValid)
            {
                _context.Notlar.Add(not);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            KategorileriYukle();
            return View(not);
        }

        public IActionResult Duzenle(int id)
        {
            var not = _context.Notlar.Find(id);
            if (not == null) return NotFound();

            KategorileriYukle();
            return View(not);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(Notum not)
        {
            if (ModelState.IsValid)
            {
                _context.Notlar.Update(not);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            KategorileriYukle();
            return View(not);
        }

        public IActionResult Sil(int id)
        {
            var not = _context.Notlar.Include(n => n.Kategori).FirstOrDefault(n => n.Id == id);
            if (not == null) return NotFound();

            return View(not);
        }

        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SilOnay(int id)
        {
            var not = await _context.Notlar.FindAsync(id);
            if (not != null)
            {
                _context.Notlar.Remove(not);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}