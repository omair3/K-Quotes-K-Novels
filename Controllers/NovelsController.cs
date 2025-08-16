// Controllers/NovelsController.cs
using KQuotesNovels.Data;
using KQuotesNovels.Models;
using KQuotesNovels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KQuotesNovels.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class NovelsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public NovelsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // PUBLIC browse
        [AllowAnonymous]
        public async Task<IActionResult> Index()
            => View(await _db.Novels.Include(n => n.Author).ToListAsync());

        // PUBLIC details 
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var novel = await _db.Novels
                .Include(n => n.Author)
                .Include(n => n.NovelGenres).ThenInclude(ng => ng.Genre)
                .Include(n => n.NovelDramas).ThenInclude(nd => nd.Drama)
                .FirstOrDefaultAsync(n => n.NovelId == id);

            if (novel is null) return NotFound();

           
            ViewBag.GenreId = new SelectList(_db.Genres.OrderBy(g => g.Name), "GenreId", "Name");
            ViewBag.DramaId = new SelectList(_db.Dramas.OrderBy(d => d.Title), "DramaId", "Title");

            return View(novel);
        }

        //  CREATE 
        public async Task<IActionResult> Create()
        {
            var vm = new NovelFormVM
            {
                AllAuthors = await _db.Authors.OrderBy(a => a.Name).ToListAsync(),
                AllGenres = await _db.Genres.OrderBy(g => g.Name).ToListAsync(),
                AllDramas = await _db.Dramas.OrderBy(d => d.Title).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NovelFormVM vm)
        {
            if (!ModelState.IsValid)
            {
              
                vm.AllAuthors = await _db.Authors.OrderBy(a => a.Name).ToListAsync();
                vm.AllGenres = await _db.Genres.OrderBy(g => g.Name).ToListAsync();
                vm.AllDramas = await _db.Dramas.OrderBy(d => d.Title).ToListAsync();
                return View(vm);
            }

            var novel = new Novel { Title = vm.Title, AuthorId = vm.AuthorId };
            _db.Novels.Add(novel);
            await _db.SaveChangesAsync();

            // associations
            foreach (var gid in vm.SelectedGenreIds)
                _db.NovelGenres.Add(new NovelGenre { NovelId = novel.NovelId, GenreId = gid });

            foreach (var did in vm.SelectedDramaIds)
                _db.NovelDramas.Add(new NovelDrama { NovelId = novel.NovelId, DramaId = did });

            // image upload (optional field ImageFile)
            if (vm.ImageFile != null)
            {
                var path = await SaveImageAsync(vm.ImageFile, "novels");
                if (path != null) novel.ImagePath = path;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // EDIT 
        public async Task<IActionResult> Edit(int id)
        {
            var novel = await _db.Novels
                .Include(n => n.NovelGenres)
                .Include(n => n.NovelDramas)
                .FirstOrDefaultAsync(n => n.NovelId == id);

            if (novel is null) return NotFound();

            var vm = new NovelFormVM
            {
                NovelId = novel.NovelId,
                Title = novel.Title,
                AuthorId = novel.AuthorId,
                SelectedGenreIds = novel.NovelGenres.Select(x => x.GenreId).ToList(),
                SelectedDramaIds = novel.NovelDramas.Select(x => x.DramaId).ToList(),
                AllAuthors = await _db.Authors.OrderBy(a => a.Name).ToListAsync(),
                AllGenres = await _db.Genres.OrderBy(g => g.Name).ToListAsync(),
                AllDramas = await _db.Dramas.OrderBy(d => d.Title).ToListAsync(),
                ExistingImagePath = novel.ImagePath
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NovelFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AllAuthors = await _db.Authors.OrderBy(a => a.Name).ToListAsync();
                vm.AllGenres = await _db.Genres.OrderBy(g => g.Name).ToListAsync();
                vm.AllDramas = await _db.Dramas.OrderBy(d => d.Title).ToListAsync();
                return View(vm);
            }

            var novel = await _db.Novels
                .Include(n => n.NovelGenres)
                .Include(n => n.NovelDramas)
                .FirstOrDefaultAsync(n => n.NovelId == vm.NovelId);

            if (novel is null) return NotFound();

            novel.Title = vm.Title;
            novel.AuthorId = vm.AuthorId;

            novel.NovelGenres.Clear();
            foreach (var gid in vm.SelectedGenreIds)
                novel.NovelGenres.Add(new NovelGenre { NovelId = novel.NovelId, GenreId = gid });

            novel.NovelDramas.Clear();
            foreach (var did in vm.SelectedDramaIds)
                novel.NovelDramas.Add(new NovelDrama { NovelId = novel.NovelId, DramaId = did });

            if (vm.ImageFile != null)
            {
                var path = await SaveImageAsync(vm.ImageFile, "novels");
                if (path != null) novel.ImagePath = path;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //  DELETE 
        public async Task<IActionResult> Delete(int id)
        {
            var n = await _db.Novels.FindAsync(id);
            return n is null ? NotFound() : View(n);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var n = await _db.Novels.FindAsync(id);
            if (n != null)
            {
                _db.Novels.Remove(n);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public async Task<IActionResult> AddGenre(int id, int genreId)
        {
            if (await _db.NovelGenres.FindAsync(id, genreId) is null)
                _db.NovelGenres.Add(new NovelGenre { NovelId = id, GenreId = genreId });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveGenre(int id, int genreId)
        {
            var link = await _db.NovelGenres.FindAsync(id, genreId);
            if (link != null) _db.NovelGenres.Remove(link);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> AddDrama(int id, int dramaId)
        {
            if (await _db.NovelDramas.FindAsync(id, dramaId) is null)
                _db.NovelDramas.Add(new NovelDrama { NovelId = id, DramaId = dramaId });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveDrama(int id, int dramaId)
        {
            var link = await _db.NovelDramas.FindAsync(id, dramaId);
            if (link != null) _db.NovelDramas.Remove(link);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        // ===== helper =====
        private async Task<string?> SaveImageAsync(IFormFile file, string subfolder)
        {
            if (file == null || file.Length == 0) return null;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png" };
            if (!allowed.Contains(ext)) return null;

            var webroot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var folder = Path.Combine(webroot, "images", subfolder);
            Directory.CreateDirectory(folder);

            var fname = $"{Guid.NewGuid():N}{ext}";
            var full = Path.Combine(folder, fname);

            using (var stream = new FileStream(full, FileMode.Create))
                await file.CopyToAsync(stream);

            // return relative web path for <img src="">
            return $"/images/{subfolder}/{fname}";
        }
    }
}
