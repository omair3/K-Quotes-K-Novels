using System.ComponentModel.DataAnnotations;
namespace KQuotesNovels.Models
{
    public class Novel
    {
        public int NovelId { get; set; }
        [Required, StringLength(120)] public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public List<NovelGenre> NovelGenres { get; set; } = new();
        public List<NovelDrama> NovelDramas { get; set; } = new();
        public List<NovelQuote> NovelQuotes { get; set; } = new();
    }
}

