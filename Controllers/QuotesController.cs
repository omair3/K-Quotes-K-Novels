using KQuotesNovels.Data;
using KQuotesNovels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KQuotesNovels.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class QuotesController : Controller
    {
        private readonly AppDbContext _db;
        public QuotesController(AppDbContext db) { _db = db; }

        [AllowAnonymous]
        public async Task<IActionResult> Index() => View(await _db.Quotes.Include(q => q.Drama).ToListAsync());

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var q = await _db.Quotes.Include(q => q.Drama).FirstOrDefaultAsync(q => q.QuoteId == id);
            return q is null ? NotFound() : View(q);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Quote q)
        {
            if (!ModelState.IsValid) return View(q);
            _db.Add(q);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var q = await _db.Quotes.FindAsync(id);
            return q is null ? NotFound() : View(q);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Quote q)
        {
            if (!ModelState.IsValid) return View(q);
            _db.Update(q);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var q = await _db.Quotes.FindAsync(id);
            return q is null ? NotFound() : View(q);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var q = await _db.Quotes.FindAsync(id);
            if (q != null)
            {
                _db.Quotes.Remove(q);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
