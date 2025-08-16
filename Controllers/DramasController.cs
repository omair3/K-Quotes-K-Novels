using KQuotesNovels.Data;
using KQuotesNovels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KQuotesNovels.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class DramasController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public DramasController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() => View(await _db.Dramas.ToListAsync());

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var m = await _db.Dramas.FindAsync(id);
            return m is null ? NotFound() : View(m);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Drama m, IFormFile? imageFile)
        {
            if (!ModelState.IsValid) return View(m);

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images/dramas");
                Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                m.ImagePath = "/images/dramas/" + fileName;
            }

            _db.Add(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var m = await _db.Dramas.FindAsync(id);
            return m is null ? NotFound() : View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Drama m, IFormFile? imageFile)
        {
            if (!ModelState.IsValid) return View(m);

            var existing = await _db.Dramas.AsNoTracking().FirstOrDefaultAsync(x => x.DramaId == m.DramaId);
            if (existing == null) return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images/dramas");
                Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                m.ImagePath = "/images/dramas/" + fileName;
            }
            else
            {
                m.ImagePath = existing.ImagePath; // keep old image
            }

            _db.Update(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Dramas.FindAsync(id);
            return m is null ? NotFound() : View(m);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var m = await _db.Dramas.FindAsync(id);
            if (m != null)
            {
                _db.Dramas.Remove(m);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
