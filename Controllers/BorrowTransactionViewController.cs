using LibraryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Models.ViewModels;

namespace LibraryApp.Controllers
{
    public class BorrowTransactionViewController : Controller
    {
        private readonly IBorrowTransactionRepository _borrowTransactionRepository;
        private readonly IBookRepository _bookRepository;
        public BorrowTransactionViewController(IBorrowTransactionRepository borrowTransactionRepository, IBookRepository bookRepository)
        {
            _borrowTransactionRepository = borrowTransactionRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet("/borrowings")]
        public IActionResult DisplayBorrowingList()
        {
            var borrows = _borrowTransactionRepository.GetBorrowTransactions();
            var borrowingList = new List<BorrowingListViewModel>();

            foreach (var borrow in borrows)
            {
                var book = _bookRepository.GetBookById(borrow.BorrowedBook); 
                if(book == null)
                {
                    break;
                }
                borrowingList.Add(new BorrowingListViewModel
                {
                    Id = borrow.Id,
                    Surname = borrow.Surname,
                    BorrowDate = borrow.BorrowDate,
                    DueDate = borrow.DueDate,
                    ReturnDate = borrow.ReturnDate,
                    Status = borrow.Status,
                    BookTitle = book.Title 
                });
            }

            return View("~/Views/Home/DisplayBorrowingList.cshtml", borrowingList);
        }

        [HttpGet("/borrow")]
        public IActionResult AddTransaction()
        {
            var id = HttpContext.Request.Query["id"].ToString();
            ViewBag.Id = id;
            var title = HttpContext.Request.Query["title"].ToString();
            ViewBag.Title = title;
            return View("~/Views/Home/BorrowBook.cshtml");
        }

        [HttpGet("/borrowings/borrowdate/search")]
        public IActionResult GetBorrowedTransactionsOverTimeByBorrowDate([FromQuery] string startDate = null, [FromQuery] string endDate = null)
        {
            var borrowingList = new List<BorrowingListViewModel>();

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                ViewBag.ErrorMessage = "Both startDate and endDate must be provided.";
                return View("~/Views/Home/DisplayBorrowingList.cshtml");
            }

            if (!DateTime.TryParse(startDate, out DateTime startDateFormat))
            {
                ViewBag.ErrorMessage = $"Invalid start date format: {startDate}. Expected format is yyyy-MM-dd.";
                return View("~/Views/Home/DisplayBorrowingList.cshtml");
            }

            if (!DateTime.TryParse(endDate, out DateTime endDateFormat))
            {
                ViewBag.ErrorMessage = $"Invalid end date format: {endDate}. Expected format is yyyy-MM-dd.";
                return View("~/Views/Home/DisplayBorrowingList.cshtml");
            }

            try
            {
                var borrows = _borrowTransactionRepository.GetBorrowedTransactionsOverTimeByBorrowedDate(startDateFormat, endDateFormat);
                foreach (var borrow in borrows)
                {
                    var book = _bookRepository.GetBookById(borrow.BorrowedBook);
                    borrowingList.Add(new BorrowingListViewModel
                    {
                        Id = borrow.Id,
                        Surname = borrow.Surname,
                        BorrowDate = borrow.BorrowDate,
                        DueDate = borrow.DueDate,
                        ReturnDate = borrow.ReturnDate,
                        Status = borrow.Status,
                        BookTitle = book.Title
                    });
                }
                return View("~/Views/Home/DisplayBorrowingList.cshtml", borrowingList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("~/Views/Home/DisplayBorrowingList.cshtml");
            }
        }
        [HttpGet("/borrowings/duedate/search")]
        public IActionResult GetBorrowedTransactionOverTimeByDueDate([FromQuery] string startDate = null, [FromQuery] string endDate = null)
        {
            var borrowingList = new List<BorrowingListViewModel>();

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                ViewBag.ErrorMessage = "Both startDate and endDate must be provided.";
                return View("~/Views/Home/DisplayBorrowingList.cshtml");
            }

            if (!DateTime.TryParse(startDate, out DateTime startDateFormat))
            {
                ViewBag.ErrorMessage = $"Invalid start date format: {startDate}. Expected format is yyyy-MM-dd.";
                return View("~/Views/Home/DisplayBorrowingList.cshtml");
            }

            if (!DateTime.TryParse(endDate, out DateTime endDateFormat))
            {
                ViewBag.ErrorMessage = $"Invalid end date format: {endDate}. Expected format is yyyy-MM-dd.";
                return View("~/Views/Home/DisplayBorrowingList.cshtml");
            }

            try
            {
                var borrows = _borrowTransactionRepository.GetBorrowedTransactionsOverTimeByDueDate(startDateFormat, endDateFormat);
                foreach (var borrow in borrows)
                {
                    var book = _bookRepository.GetBookById(borrow.BorrowedBook);
                    borrowingList.Add(new BorrowingListViewModel
                    {
                        Id = borrow.Id,
                        Surname = borrow.Surname,
                        BorrowDate = borrow.BorrowDate,
                        DueDate = borrow.DueDate,
                        ReturnDate = borrow.ReturnDate,
                        Status = borrow.Status,
                        BookTitle = book.Title
                    });
                }
                return View("~/Views/Home/DisplayBorrowingList.cshtml", borrowingList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("~/Views/Home/DisplayBorrowingList.cshtml");
            }
            
        }

    }
}
