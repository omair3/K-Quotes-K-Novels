using System.ComponentModel.DataAnnotations;
namespace KQuotesNovels.Models
{
    public class Drama
    {
        public int DramaId { get; set; }
        [Required, StringLength(120)] public string Title { get; set; } = string.Empty;
        public List<Quote> Quotes { get; set; } = new();
        public List<NovelDrama> NovelDramas { get; set; } = new();
        public string? ImagePath { get; set; }

    }
}

