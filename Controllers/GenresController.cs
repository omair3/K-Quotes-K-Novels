using KQuotesNovels.Data;
using KQuotesNovels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KQuotesNovels.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class GenresController : Controller
    {
        private readonly AppDbContext _db;
        public GenresController(AppDbContext db) { _db = db; }

        [AllowAnonymous]
        public async Task<IActionResult> Index() => View(await _db.Genres.ToListAsync());

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var g = await _db.Genres.FindAsync(id);
            return g is null ? NotFound() : View(g);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Genre g)
        {
            if (!ModelState.IsValid) return View(g);
            _db.Add(g);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var g = await _db.Genres.FindAsync(id);
            return g is null ? NotFound() : View(g);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Genre g)
        {
            if (!ModelState.IsValid) return View(g);
            _db.Update(g);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var g = await _db.Genres.FindAsync(id);
            return g is null ? NotFound() : View(g);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var g = await _db.Genres.FindAsync(id);
            if (g != null)
            {
                _db.Genres.Remove(g);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
