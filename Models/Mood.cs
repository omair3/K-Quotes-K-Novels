using System.ComponentModel.DataAnnotations;
namespace KQuotesNovels.Models
{
    public class Mood
    {
        public int MoodId { get; set; }
        [Required, StringLength(50)] public string Name { get; set; } = string.Empty;
        public List<QuoteMood> QuoteMoods { get; set; } = new();
    }
}

