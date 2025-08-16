using KQuotesNovels.Models;
using Microsoft.EntityFrameworkCore;

namespace KQuotesNovels.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Novel> Novels => Set<Novel>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Drama> Dramas => Set<Drama>();
        public DbSet<Quote> Quotes => Set<Quote>();
        public DbSet<Mood> Moods => Set<Mood>();

        public DbSet<NovelGenre> NovelGenres => Set<NovelGenre>();
        public DbSet<QuoteMood> QuoteMoods => Set<QuoteMood>();
        public DbSet<NovelDrama> NovelDramas => Set<NovelDrama>();
        public DbSet<NovelQuote> NovelQuotes => Set<NovelQuote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NovelGenre>().HasKey(x => new { x.NovelId, x.GenreId });
            modelBuilder.Entity<QuoteMood>().HasKey(x => new { x.QuoteId, x.MoodId });
            modelBuilder.Entity<NovelDrama>().HasKey(x => new { x.NovelId, x.DramaId });
            modelBuilder.Entity<NovelQuote>().HasKey(x => new { x.NovelId, x.QuoteId });

            modelBuilder.Entity<Novel>()
                .HasOne(n => n.Author)
                .WithMany(a => a.Novels)
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Drama)
                .WithMany(d => d.Quotes)
                .HasForeignKey(q => q.DramaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

