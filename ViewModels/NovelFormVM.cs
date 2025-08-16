using KQuotesNovels.Models;
using System.ComponentModel.DataAnnotations;

namespace KQuotesNovels.ViewModels
{
    public class NovelFormVM
    {
        public int NovelId { get; set; }
        [Required, StringLength(120)] public string Title { get; set; } = string.Empty;
        [Required] public int AuthorId { get; set; }
        public List<int> SelectedGenreIds { get; set; } = new();
        public List<int> SelectedDramaIds { get; set; } = new();
        public List<Genre> AllGenres { get; set; } = new();
        public List<Drama> AllDramas { get; set; } = new();
        public List<Author> AllAuthors { get; set; } = new();
    }
}

