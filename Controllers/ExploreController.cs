using KQuotesNovels.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KQuotesNovels.Controllers
{
    public class ExploreController : Controller
    {
        private readonly AppDbContext _db;
        public ExploreController(AppDbContext db){ _db = db; }

        public async Task<IActionResult> Index(string? tag)
        {
            ViewBag.Tags = await _db.Genres.Select(g=>g.Name).Union(_db.Moods.Select(m=>m.Name)).OrderBy(s=>s).ToListAsync();
            if (string.IsNullOrWhiteSpace(tag))
            {
                ViewBag.SelectedTag = null;
                ViewBag.Novels = new List<object>();
                ViewBag.Quotes = new List<object>();
                return View();
            }
            ViewBag.SelectedTag = tag;
            ViewBag.Novels = await _db.NovelGenres.Include(ng=>ng.Novel).ThenInclude(n=>n.Author).Include(ng=>ng.Genre)
                .Where(ng=>ng.Genre!.Name==tag).Select(ng=>ng.Novel!).ToListAsync();
            ViewBag.Quotes = await _db.QuoteMoods.Include(qm=>qm.Quote).ThenInclude(q=>q.Drama).Include(qm=>qm.Mood)
                .Where(qm=>qm.Mood!.Name==tag).Select(qm=>qm.Quote!).ToListAsync();
            return View();
        }
    }
}

