using KQuotesNovels.Models;

namespace KQuotesNovels.Data
{
    public static class SeedData
    {
        public static void EnsureSeeded(AppDbContext db)
        {
            if (db.Authors.Any()) return;

            var authors = new[] { "Shin Kyung-sook","Han Kang","Hwang Sok-yong","Kim Young-ha","Yi Mun-yol","Cho Nam-joo","Gong Ji-young","Bae Su-ah","Kim Hoon","Park Min-gyu"}
                .Select(n=> new Author{ Name=n}).ToArray();

            var genres = new[] { "Romance","Historical","Fantasy","Thriller","Comedy","Melodrama","Sad","Action","Mystery","Youth" }
                .Select(n=> new Genre{ Name=n}).ToArray();

            var novels = new[] {
                new Novel { Title = "Please Look After Mom", Author = authors[0] },
                new Novel { Title = "The Vegetarian", Author = authors[1] },
                new Novel { Title = "At Dusk", Author = authors[2] },
                new Novel { Title = "I Have the Right to Destroy Myself", Author = authors[3] },
                new Novel { Title = "Our Twisted Hero", Author = authors[4] },
                new Novel { Title = "Kim Jiyoung, Born 1982", Author = authors[5] },
                new Novel { Title = "Our Happy Time", Author = authors[6] },
                new Novel { Title = "A Greater Music", Author = authors[7] },
                new Novel { Title = "Song of the Sword", Author = authors[8] },
                new Novel { Title = "Pavane for a Dead Princess", Author = authors[9] },
            };

            var dramas = new[] { "Goblin","Crash Landing on You","Mr. Sunshine","Reply 1988","Kingdom","My Mister","It's Okay to Not Be Okay","Descendants of the Sun","Signal","Vincenzo" }
                .Select(t=> new Drama{ Title=t}).ToArray();

            var moods = new[] { "Romance","Historical","Fantasy","Thriller","Comedy","Melodrama","Sad","Action","Mystery","Youth" }
                .Select(n=> new Mood{ Name=n}).ToArray();

            var quotes = new[] {
                new Quote { Text = "Even in the coldest night, a small light waits.", Drama = dramas[0] },
                new Quote { Text = "Love crosses the borders we draw.", Drama = dramas[1] },
                new Quote { Text = "The past does not fade; it asks for courage.", Drama = dramas[2] },
                new Quote { Text = "Family is the warmest street in the neighborhood.", Drama = dramas[3] },
                new Quote { Text = "When fear rises, so must resolve.", Drama = dramas[4] },
                new Quote { Text = "Kindness is louder than despair.", Drama = dramas[5] },
                new Quote { Text = "Healing begins when we see each other clearly.", Drama = dramas[6] },
                new Quote { Text = "Duty is heavy, but the heart is heavier.", Drama = dramas[7] },
                new Quote { Text = "Truth echoes across time if we listen.", Drama = dramas[8] },
                new Quote { Text = "Justice smiles when patience wins.", Drama = dramas[9] },
            };

            db.AddRange(authors); db.AddRange(genres); db.AddRange(novels);
            db.AddRange(dramas); db.AddRange(moods); db.AddRange(quotes);
            db.SaveChanges();

            void NG(Novel n, string g){ db.NovelGenres.Add(new NovelGenre{ NovelId=n.NovelId, GenreId=db.Genres.First(x=>x.Name==g).GenreId});}
            NG(novels[0], "Melodrama"); NG(novels[0], "Sad");
            NG(novels[1], "Melodrama"); NG(novels[1], "Mystery");
            NG(novels[2], "Romance"); NG(novels[2], "Sad");
            NG(novels[3], "Thriller");
            NG(novels[4], "Youth"); NG(novels[4], "Mystery");
            NG(novels[5], "Melodrama"); NG(novels[5], "Romance");
            NG(novels[6], "Sad"); NG(novels[6], "Romance");
            NG(novels[7], "Romance");
            NG(novels[8], "Historical"); NG(novels[8], "Action");
            NG(novels[9], "Melodrama");

            void ND(Novel n, string d){ db.NovelDramas.Add(new NovelDrama{ NovelId=n.NovelId, DramaId=db.Dramas.First(x=>x.Title==d).DramaId});}
            ND(novels[0], "My Mister"); ND(novels[1], "It's Okay to Not Be Okay"); ND(novels[2], "Mr. Sunshine");
            ND(novels[3], "Vincenzo"); ND(novels[4], "Reply 1988"); ND(novels[5], "Crash Landing on You");
            ND(novels[6], "Goblin"); ND(novels[7], "Signal"); ND(novels[8], "Mr. Sunshine"); ND(novels[9], "Descendants of the Sun");

            void QM(Quote q, string m){ db.QuoteMoods.Add(new QuoteMood{ QuoteId=q.QuoteId, MoodId=db.Moods.First(x=>x.Name==m).MoodId}); }
            QM(quotes[0], "Fantasy"); QM(quotes[0], "Romance");
            QM(quotes[1], "Romance"); QM(quotes[1], "Comedy");
            QM(quotes[2], "Historical"); QM(quotes[2], "Melodrama");
            QM(quotes[3], "Youth"); QM(quotes[3], "Comedy");
            QM(quotes[4], "Thriller"); QM(quotes[4], "Action");
            QM(quotes[5], "Melodrama"); QM(quotes[5], "Sad");
            QM(quotes[6], "Melodrama"); QM(quotes[6], "Romance");
            QM(quotes[7], "Action"); QM(quotes[7], "Romance");
            QM(quotes[8], "Mystery"); QM(quotes[8], "Historical");
            QM(quotes[9], "Action"); QM(quotes[9], "Comedy");

            db.SaveChanges();
        }
    }
}

