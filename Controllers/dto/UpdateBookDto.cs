using LibraryApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Controllers.dto
{
    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string? Author { get; set; }

        [Required(ErrorMessage = "YearPublished is required.")]
        public string? YearPublished { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        public string? Genre { get; set; }

        [Required(ErrorMessage = "Publisher is required.")]
        public string? Publisher { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "TotalCopies are required")]
        [Range(0, int.MaxValue, ErrorMessage = "TotalCopies could not be negative number")]
        public int? TotalCopies { get; set; }

        [Required(ErrorMessage = "AvailableCopies are required")]
        [Range(0, int.MaxValue, ErrorMessage = "AvailableCopies could not be negative number")]
        public int? AvailableCopies { get; set; }

    }
}
