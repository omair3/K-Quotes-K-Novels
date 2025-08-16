using System.ComponentModel.DataAnnotations;
namespace KQuotesNovels.Models
{
    public class Quote
    {
        public int QuoteId { get; set; }
        [Required, StringLength(240)] public string Text { get; set; } = string.Empty;
        public int DramaId { get; set; }
        public Drama? Drama { get; set; }
        public List<QuoteMood> QuoteMoods { get; set; } = new();
        public List<NovelQuote> NovelQuotes { get; set; } = new();
    }
}

