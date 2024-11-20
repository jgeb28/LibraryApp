using LibraryApp.Controllers.dto;
using LibraryApp.Models;
using LibraryApp.Repositories;
using LibraryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowTransactionController : Controller
    {
        private readonly IBorrowTransactionRepository _borrowTransactionRepository;
        private readonly IBookRepository _bookRepository;

        public BorrowTransactionController(IBorrowTransactionRepository borrowTransactionRepository, IBookRepository bookRepository)
        {
            _borrowTransactionRepository = borrowTransactionRepository;
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAllTransaction()
        {
            var transactions = _borrowTransactionRepository.GetBorrowTransactions();
            return Ok(transactions);
        }

        [HttpPost("add")]
        public IActionResult CreateBorrowTransaction([FromForm] CreateBorrowTransactionDto createBorrowTransactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState });
            }

            if (createBorrowTransactionDto.BorrowedBook == null)
            {
                return BadRequest(new { message = "The BorrowedBook is required" } );
            }

            try
            {
                var book = _bookRepository.GetBookById(createBorrowTransactionDto.BorrowedBook.Value);
                if (book == null)
                {
                    return BadRequest(new { message = "There is not book with that ID: {createBorrowTransactionDto.BorrowedBook}"});
                }

                if (!book.isAvailable())
                {
                    return BadRequest(new { message = "The book with ID: {book.Id} is not currently available"});
                }

                _borrowTransactionRepository.CreateTransaction(book, createBorrowTransactionDto.Surname);
                _bookRepository.UpdateBook(book.Id, new UpdateBookDto { AvailableCopies = book.AvailableCopies - 1 });

                return Ok(new { message = "Poprawnie wypożyczono", newBook = book });

            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message });
            }
        }

        [HttpGet("borrowdate/search")]
        public IActionResult GetBorrowedTransactionsOverTimeByBorrowDate([FromQuery] string startDate = null, [FromQuery] string endDate = null)
        {
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return BadRequest( new { message = $"Both startDate and endDate must be provided."});
            }

            if (!DateTime.TryParse(startDate, out DateTime startDateFormat))
            {
                return BadRequest(new { message = $"Invalid start date format: {startDate}. Expected format is yyyy-MM-dd." });
            }

            if (!DateTime.TryParse(endDate, out DateTime endDateFormat))
            {
                return BadRequest( new { message =  $"Invalid end date format: {endDate}. Expected format is yyyy-MM-dd." });
            }

            try
            {
                var filteredTransactions = _borrowTransactionRepository.GetBorrowedTransactionsOverTimeByBorrowedDate(startDateFormat, endDateFormat);

                return Ok(filteredTransactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message });
            }
        }
        [HttpGet("duedate/search")]
        public IActionResult GetBorrowedTransactionOverTimeByDueDate([FromQuery] string startDate = null, [FromQuery] string endDate = null)
        {
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return BadRequest(new { message = $"Both startDate and endDate must be provided." });
            }

            if (!DateTime.TryParse(startDate, out DateTime startDateFormat))
            {
                return BadRequest(new { message = $"Invalid start date format: {startDate}. Expected format is yyyy-MM-dd." });
            }

            if (!DateTime.TryParse(endDate, out DateTime endDateFormat))
            {
                return BadRequest(new { message = $"Invalid end date format: {endDate}. Expected format is yyyy-MM-dd." });
            }

            try
            {
                var filteredTransactions = _borrowTransactionRepository.GetBorrowedTransactionsOverTimeByDueDate(startDateFormat, endDateFormat);

                return Ok(filteredTransactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message });
            }
        }


        [HttpPut("return/{id}")]
        public IActionResult ReturnBook(Guid id)
        {
            try
            {
                var transaction = _borrowTransactionRepository.GetTransactionById(id);
                Console.WriteLine(transaction);
                if (transaction == null)
                {
                    return BadRequest(new { message = $"There is not transaction with that ID: {id}"});
                }
                var book = _bookRepository.GetBookById(transaction.BorrowedBook);
                if (book == null)
                {
                    return BadRequest(new { message = "This book does not exist"});
                }
                _bookRepository.UpdateBook(book.Id, new UpdateBookDto { AvailableCopies = book.AvailableCopies + 1 });
                _borrowTransactionRepository.UpdateTransactionStatus(id, StatusEnum.Returned);
                return Ok(new { message = $"Book was successfully returned." }); ;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message});
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBorrowTransaction(Guid id)
        {
            try
            {
                var transaction = _borrowTransactionRepository.GetTransactionById(id);
                if (transaction == null)
                {
                    return NotFound(new { message =  $"There is not a transaction with ID: {id}"});
                }

                var borrowedBook = _bookRepository.GetBookById(transaction.BorrowedBook);
                _bookRepository.UpdateBook(borrowedBook.Id, new UpdateBookDto { AvailableCopies = borrowedBook.AvailableCopies + 1 });

                _borrowTransactionRepository.DeleteTransaction(id);
                return Ok(new { message = $"Transaction was successfully deleted." }); ;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}