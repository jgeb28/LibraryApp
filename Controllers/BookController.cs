using LibraryApp.Controllers.dto;
using LibraryApp.Models;
using LibraryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace LibraryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowTransactionRepository _borrowTransactionRepository;

        public BookController(IBookRepository bookRepository, IBorrowTransactionRepository borrowTransactionRepository)
        {
            _bookRepository = bookRepository;
            _borrowTransactionRepository = borrowTransactionRepository;
        }

        [HttpGet("all")]
        public IActionResult GetAllBooks()
        {
            var books = _bookRepository.GetAllBooks();
            return Ok(books);
        }

        [HttpPost("add")]
        public IActionResult AddNewBook([FromForm] CreateBookDto createBookDto)
        {
            var book = new BookModel
            {
                Title = createBookDto.Title,
                Author = createBookDto.Author,
                YearPublished = createBookDto.YearPublished != null ? createBookDto.YearPublished : "Unknown",
                Genre = createBookDto.Genre,
                Publisher = createBookDto.Publisher,
                Description = createBookDto.Description,
                TotalCopies = createBookDto.TotalCopies,
                AvailableCopies = createBookDto.TotalCopies,
                Status = "Available"
            };
            try
            {
                _bookRepository.AddNewBook(book);
                return Ok(new { message = "Book added successfully", newBook = book });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while adding the book: " + ex.Message } );
            }
        }


        [HttpPut("update/{id}")]
        public IActionResult UpdateBook(Guid id,[FromForm] UpdateBookDto updateBookDto)
        {
            try
            {
                _bookRepository.UpdateBook(id, updateBookDto);
                return Ok(new { message = "Book updated successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBook(Guid id)
        {
            try
            {
                var transactions = _borrowTransactionRepository.GetBorrowTransactionsByBookId(id);
                bool canBeDeleted = true;
                foreach (var transaction in transactions)
                {
                    if (transaction.ReturnDate == null)
                    {
                        canBeDeleted = false;
                    }
                }
                if (!canBeDeleted)
                {
                    return BadRequest(new { message = $"Book cannot be deleted because it is borrowed." });
                }

                foreach (var transaction in transactions)
                {
                    _borrowTransactionRepository.DeleteTransaction(transaction.Id);
                }
                _bookRepository.DeleteBook(id);
                return Ok(new { message = $"Book was successfully deleted." });
            }
            catch (Exception ex)
            {
                return BadRequest(new{message = ex.Message});
            }
        }
    }
}
