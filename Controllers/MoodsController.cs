using KQuotesNovels.Data;
using KQuotesNovels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KQuotesNovels.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class MoodsController : Controller
    {
        private readonly AppDbContext _db;
        public MoodsController(AppDbContext db) { _db = db; }

        [AllowAnonymous]
        public async Task<IActionResult> Index() => View(await _db.Moods.ToListAsync());

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var m = await _db.Moods.FindAsync(id);
            return m is null ? NotFound() : View(m);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Mood m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Add(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var m = await _db.Moods.FindAsync(id);
            return m is null ? NotFound() : View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Mood m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Update(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Moods.FindAsync(id);
            return m is null ? NotFound() : View(m);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var m = await _db.Moods.FindAsync(id);
            if (m != null)
            {
                _db.Moods.Remove(m);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
