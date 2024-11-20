using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Controllers.dto
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public required string Author { get; set; }

        public string? YearPublished { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        public required string Genre { get; set; }

        [Required(ErrorMessage = "Publisher is required.")]
        public required string Publisher { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "TotalCopies are required")]
        [Range(0, int.MaxValue, ErrorMessage = "TotalCopies could not be negative number")]
        public required int TotalCopies { get; set; }

    }



}

