using KQuotesNovels.Data;
using KQuotesNovels.Models;
using KQuotesNovels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KQuotesNovels.Controllers
{
    public class NovelsController : Controller
    {
        private readonly AppDbContext _db;
        public NovelsController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
            => View(await _db.Novels.Include(n => n.Author).ToListAsync());

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

            // Similar quotes: Novel Genre name matches Quote Mood name
            var tags = novel.NovelGenres.Select(g => g.Genre!.Name).ToList();
            ViewBag.SimilarQuotes = await _db.QuoteMoods
                .Include(qm => qm.Quote).ThenInclude(q => q.Drama)
                .Include(qm => qm.Mood)
                .Where(qm => tags.Contains(qm.Mood!.Name))
                .Select(qm => qm.Quote!).Distinct()
                .ToListAsync();

            return View(novel);
        }

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

            foreach (var gid in vm.SelectedGenreIds)
                _db.NovelGenres.Add(new NovelGenre { NovelId = novel.NovelId, GenreId = gid });

            foreach (var did in vm.SelectedDramaIds)
                _db.NovelDramas.Add(new NovelDrama { NovelId = novel.NovelId, DramaId = did });

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

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
                SelectedGenreIds = novel.NovelGenres.Select(g => g.GenreId).ToList(),
                SelectedDramaIds = novel.NovelDramas.Select(d => d.DramaId).ToList(),
                AllAuthors = await _db.Authors.OrderBy(a => a.Name).ToListAsync(),
                AllGenres = await _db.Genres.OrderBy(g => g.Name).ToListAsync(),
                AllDramas = await _db.Dramas.OrderBy(d => d.Title).ToListAsync()
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

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ----- Association actions (fixed) -----

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

        // ----- Delete (GET/POST) -----

        public async Task<IActionResult> Delete(int id)
        {
            var novel = await _db.Novels
                .Include(n => n.Author)
                .FirstOrDefaultAsync(n => n.NovelId == id);
            return novel is null ? NotFound() : View(novel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var novel = await _db.Novels.FindAsync(id);
            if (novel != null)
            {
                _db.Novels.Remove(novel);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
