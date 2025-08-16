using System.ComponentModel.DataAnnotations;
namespace KQuotesNovels.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        [Required, StringLength(50)] public string Name { get; set; } = string.Empty;
        public List<NovelGenre> NovelGenres { get; set; } = new();
    }
}

