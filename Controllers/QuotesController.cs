using KQuotesNovels.Data;
using KQuotesNovels.Models;
using KQuotesNovels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KQuotesNovels.Controllers
{
    public class QuotesController : Controller
    {
        private readonly AppDbContext _db;
        public QuotesController(AppDbContext db){ _db=db; }

        public async Task<IActionResult> Index()=> View(await _db.Quotes.Include(q=>q.Drama).ToListAsync());

        public async Task<IActionResult> Details(int id)
        {
            var quote = await _db.Quotes.Include(q=>q.Drama).Include(q=>q.QuoteMoods).ThenInclude(qm=>qm.Mood).FirstOrDefaultAsync(q=>q.QuoteId==id);
            if(quote is null) return NotFound();
            ViewBag.MoodId = new SelectList(_db.Moods.OrderBy(m=>m.Name),"MoodId","Name");
            return View(quote);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new QuoteFormVM{
                AllDramas = await _db.Dramas.OrderBy(d=>d.Title).ToListAsync(),
                AllMoods = await _db.Moods.OrderBy(m=>m.Name).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuoteFormVM vm)
        {
            if(!ModelState.IsValid){
                vm.AllDramas = await _db.Dramas.OrderBy(d=>d.Title).ToListAsync();
                vm.AllMoods = await _db.Moods.OrderBy(m=>m.Name).ToListAsync();
                return View(vm);
            }
            var quote = new Quote{ Text=vm.Text, DramaId=vm.DramaId };
            _db.Quotes.Add(quote); await _db.SaveChangesAsync();
            foreach(var mid in vm.SelectedMoodIds) _db.QuoteMoods.Add(new QuoteMood{ QuoteId=quote.QuoteId, MoodId=mid });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var quote = await _db.Quotes.Include(q=>q.QuoteMoods).FirstOrDefaultAsync(q=>q.QuoteId==id);
            if(quote is null) return NotFound();
            var vm = new QuoteFormVM{
                QuoteId = quote.QuoteId, Text = quote.Text, DramaId = quote.DramaId,
                SelectedMoodIds = quote.QuoteMoods.Select(m=>m.MoodId).ToList(),
                AllDramas = await _db.Dramas.OrderBy(d=>d.Title).ToListAsync(),
                AllMoods = await _db.Moods.OrderBy(m=>m.Name).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuoteFormVM vm)
        {
            if(!ModelState.IsValid){
                vm.AllDramas = await _db.Dramas.OrderBy(d=>d.Title).ToListAsync();
                vm.AllMoods = await _db.Moods.OrderBy(m=>m.Name).ToListAsync();
                return View(vm);
            }
            var quote = await _db.Quotes.Include(q=>q.QuoteMoods).FirstOrDefaultAsync(q=>q.QuoteId==vm.QuoteId);
            if(quote is null) return NotFound();
            quote.Text = vm.Text; quote.DramaId = vm.DramaId;
            quote.QuoteMoods.Clear(); foreach(var mid in vm.SelectedMoodIds) quote.QuoteMoods.Add(new QuoteMood{ QuoteId=quote.QuoteId, MoodId=mid });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost] public async Task<IActionResult> AddMood(int id, int moodId){ if(await _db.QuoteMoods.FindAsync(id,moodId) is null) _db.QuoteMoods.Add(new QuoteMood{ QuoteId=id, MoodId=moodId}); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Details), new { id }); }
        [HttpPost] public async Task<IActionResult> RemoveMood(int id, int moodId){ var link = await _db.QuoteMoods.FindAsync(id,moodId); if(link!=null) _db.QuoteMoods.Remove(link); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Details), new { id }); }

        public async Task<IActionResult> Delete(int id){ var q = await _db.Quotes.Include(q=>q.Drama).FirstOrDefaultAsync(q=>q.QuoteId==id); return q is null? NotFound(): View(q); }
        [HttpPost, ActionName("Delete")] public async Task<IActionResult> DeleteConfirmed(int id){ var q = await _db.Quotes.FindAsync(id); if(q!=null){ _db.Quotes.Remove(q); await _db.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }
    }
}

