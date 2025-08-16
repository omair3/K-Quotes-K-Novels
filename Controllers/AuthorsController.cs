using KQuotesNovels.Data;
using KQuotesNovels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KQuotesNovels.Controllers
{
    [Authorize(Policy = "AdminOnly")]   
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _db;
        public AuthorsController(AppDbContext db) { _db = db; }

        
        [AllowAnonymous]
        public async Task<IActionResult> Index() =>
            View(await _db.Authors.Include(a => a.Novels).ToListAsync());

        
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var a = await _db.Authors
                             .Include(a => a.Novels)
                             .FirstOrDefaultAsync(a => a.AuthorId == id);
            return a is null ? NotFound() : View(a);
        }

        
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Author a)
        {
            if (!ModelState.IsValid) return View(a);
            _db.Add(a);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var a = await _db.Authors.FindAsync(id);
            return a is null ? NotFound() : View(a);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Author a)
        {
            if (!ModelState.IsValid) return View(a);
            _db.Update(a);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.Authors.FindAsync(id);
            return a is null ? NotFound() : View(a);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var a = await _db.Authors.FindAsync(id);
            if (a != null)
            {
                _db.Authors.Remove(a);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
