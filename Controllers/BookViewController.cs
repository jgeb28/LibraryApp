using LibraryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace LibraryApp.Controllers
{
    public class BookViewController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookViewController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("/book/edit/{id}")]
        public IActionResult EditBook(Guid id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View("~/Views/Home/EditBook.cshtml", book);
        }

        [HttpGet("/book/add")]
        public IActionResult AddBook()
        {
            return View("~/Views/Home/AddBook.cshtml");
        }

        [HttpGet("/books")]
        public IActionResult DisplayBooks()
        {
            var books = _bookRepository.GetAllBooks();
            return View("~/Views/Home/DisplayBooks.cshtml", books);
        }

        [HttpGet("/books/search")]
        public IActionResult GetBooks([FromQuery] string title = null, [FromQuery] string author = null, [FromQuery] string genre = null)
        {
            var books = _bookRepository.GetAllBooks();

            var booksFiltered = books.AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                booksFiltered = booksFiltered.Where(book => book.Title.ToLower() == title.ToLower());
            }
            if (!string.IsNullOrEmpty(author))
            {
                booksFiltered = booksFiltered.Where(book => book.Author.ToLower() == author.ToLower());
            }
            if (!string.IsNullOrEmpty(genre))
            {
                booksFiltered = booksFiltered.Where(book => book.Genre.ToLower() == genre.ToLower());
            }
            return View("~/Views/Home/DisplayBooks.cshtml",booksFiltered.ToList());
        }

        [HttpGet("/book/{id}")]
        public IActionResult BookDetails(Guid id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            return View("~/Views/Home/BookDetails.cshtml", book);
        }

        [HttpGet("/chart")]
        public IActionResult BorrowingChart()
        {
            return View("~/Views/Home/BorrowingChart.cshtml");
        }
    }
}
