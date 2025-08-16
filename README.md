📚 KQuotes & KNovels

A small ASP.NET Core MVC project where you can browse Korean novels and K-drama quotes.
The fun part: it matches them when Novel Genre = Quote Mood (ex: Sad ↔ Sad 💔).

✨ What’s inside

👩‍💻 ASP.NET Core MVC (.NET 8)

🗄️ Entity Framework Core (SQL Server LocalDB)

🌐 Swagger for quick API testing

🛠️ Main Features

Add / Edit / Delete ➝ Authors, Novels, Dramas, Genres, Moods, Quotes

Pick multiple genres, dramas, or moods when creating items

🔎 Explore page: shows quotes + novels that share a tag

Simple API (CRUD + linking/unlinking relations)

🔗 How things connect

Author → Novels (one author can have many novels)

Drama → Quotes (quotes belong to dramas)

Many-to-many fun:

Novels ↔ Genres

Novels ↔ Dramas

Quotes ↔ Moods

🎭 Why I built it

Just practicing with MVC + EF Core while keeping it fun with K-content. The Explore page idea came from trying to link “moods” of quotes with “genres” of novels.
## 🔐 Admin-only (Finale)
- Admin login (cookie-based): protects Create/Edit/Delete + link/unlink actions.
- Public can browse and use the Explore page.

## 🖼️ Extra Feature #1: Image Upload
- Novels & Dramas support cover images (jpg/png, 2MB max).
- Stored under `/wwwroot/uploads`.


## 📝 Notes
- Minimal, student-style code (no heavy libraries).
- Based on class patterns (controllers + views + viewmodels + EF relationships).

