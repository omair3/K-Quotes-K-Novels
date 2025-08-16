using KQuotesNovels.Data;
using KQuotesNovels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KQuotesNovels.Controllers
{
    public class GenresController : Controller
    {
        private readonly AppDbContext _db;
        public GenresController(AppDbContext db){ _db=db; }

        public async Task<IActionResult> Index()=> View(await _db.Genres.ToListAsync());
        public async Task<IActionResult> Details(int id){ var m = await _db.Genres.FindAsync(id); return m is null? NotFound(): View(m); }
        public IActionResult Create()=> View();
        [HttpPost] public async Task<IActionResult> Create(Genre m){ if(!ModelState.IsValid) return View(m); _db.Add(m); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
        public async Task<IActionResult> Edit(int id){ var m = await _db.Genres.FindAsync(id); return m is null? NotFound(): View(m); }
        [HttpPost] public async Task<IActionResult> Edit(Genre m){ if(!ModelState.IsValid) return View(m); _db.Update(m); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
        public async Task<IActionResult> Delete(int id){ var m = await _db.Genres.FindAsync(id); return m is null? NotFound(): View(m); }
        [HttpPost, ActionName("Delete")] public async Task<IActionResult> DeleteConfirmed(int id){ var m = await _db.Genres.FindAsync(id); if(m!=null){ _db.Genres.Remove(m); await _db.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }
    }
}

