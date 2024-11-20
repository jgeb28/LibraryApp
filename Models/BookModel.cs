using LibraryApp.Controllers.dto;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class BookModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public string YearPublished { get; set; }

        public string Genre { get; set; }

        public string Publisher { get; set; }

        public string Description { get; set; }

        public int AvailableCopies { get; set; }

        public int TotalCopies { get; set; }

        public string Status { get; set; }

        public BookModel() { }

        public BookModel(string title, string author, string yearPublished, string genre, string publisher, string description, string status, int availableCopies, int totalCopies)
        {
            Title = title;
            Author = author;
            YearPublished = yearPublished;
            Genre = genre;
            Publisher = publisher;
            Description = description;
            Status = status;
            AvailableCopies = availableCopies;
            TotalCopies = totalCopies;
        }

        public void updateBookModel(UpdateBookDto updateBookDto)
        {
            if (updateBookDto.Title != null)
            {
                Title = updateBookDto.Title;
            }

            if (updateBookDto.Author != null)
            {
                Author = updateBookDto.Author;
            }

            if (updateBookDto.YearPublished != null)
            {
                YearPublished = updateBookDto.YearPublished;
            }

            if (updateBookDto.Genre != null)
            {
                Genre = updateBookDto.Genre;
            }

            if (updateBookDto.Publisher != null)
            {
                Publisher = updateBookDto.Publisher;
            }

            if (updateBookDto.Description != null)
            {
                Description = updateBookDto.Description;
            }

            if (updateBookDto.Status != null)
            {
                Status = updateBookDto.Status;
            }

            if (updateBookDto.TotalCopies.HasValue)
            {
                TotalCopies = updateBookDto.TotalCopies.Value;
            }

            if (updateBookDto.AvailableCopies.HasValue)
            {
                if (updateBookDto.AvailableCopies.Value > TotalCopies)
                {
                    AvailableCopies = TotalCopies;
                }
                else
                {
                    AvailableCopies = updateBookDto.AvailableCopies.Value;
                }
            }

        }

        public bool isAvailable()
        {
            return AvailableCopies > 0;
        }

        public bool Borrow()
        {
            if (AvailableCopies <= 0)
            {
                return false;
            }
            AvailableCopies--;
            return true;
        }
    }
}