using System.ComponentModel.DataAnnotations;
namespace KQuotesNovels.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        [Required, StringLength(100)] public string Name { get; set; } = string.Empty;
        public List<Novel> Novels { get; set; } = new();
    }
}

