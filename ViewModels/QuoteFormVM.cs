using KQuotesNovels.Models;
using System.ComponentModel.DataAnnotations;

namespace KQuotesNovels.ViewModels
{
    public class QuoteFormVM
    {
        public int QuoteId { get; set; }
        [Required, StringLength(240)] public string Text { get; set; } = string.Empty;
        [Required] public int DramaId { get; set; }
        public List<int> SelectedMoodIds { get; set; } = new();
        public List<Mood> AllMoods { get; set; } = new();
        public List<Drama> AllDramas { get; set; } = new();
    }
}

