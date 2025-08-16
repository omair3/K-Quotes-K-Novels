// File: ViewModels/NovelFormVM.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using KQuotesNovels.Models;

namespace KQuotesNovels.ViewModels
{
    public class NovelFormVM
    {
        public int NovelId { get; set; }

        [Required, StringLength(120)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int AuthorId { get; set; }

        // Multi-select selections
        public List<int> SelectedGenreIds { get; set; } = new();
        public List<int> SelectedDramaIds { get; set; } = new();

        // Dropdown data
        public List<Genre> AllGenres { get; set; } = new();
        public List<Drama> AllDramas { get; set; } = new();
        public List<Author> AllAuthors { get; set; } = new();

     
        public IFormFile? ImageFile { get; set; }          
        public string? ExistingImagePath { get; set; }    
    }
}
