namespace KQuotesNovels.Models
{
    public class NovelGenre
    {
        public int NovelId { get; set; }
        public Novel? Novel { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
    }

    public class QuoteMood
    {
        public int QuoteId { get; set; }
        public Quote? Quote { get; set; }
        public int MoodId { get; set; }
        public Mood? Mood { get; set; }
    }

    public class NovelDrama
    {
        public int NovelId { get; set; }
        public Novel? Novel { get; set; }
        public int DramaId { get; set; }
        public Drama? Drama { get; set; }
    }

    public class NovelQuote
    {
        public int NovelId { get; set; }
        public Novel? Novel { get; set; }
        public int QuoteId { get; set; }
        public Quote? Quote { get; set; }
    }
}

