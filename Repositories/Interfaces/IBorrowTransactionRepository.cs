using LibraryApp.Controllers.dto;
using LibraryApp.Models;

namespace LibraryApp.Repositories.Interfaces
{
    public interface IBorrowTransactionRepository
    {
        public void CreateTransaction(BookModel book, string surname);
        public void DeleteTransaction(Guid transactionId);

        public void UpdateTransactionStatus(Guid transactionId, StatusEnum newStatus);
        public BorrowTransactionModel GetTransactionById(Guid id);

        public List<BorrowTransactionModel> GetBorrowedTransactionsOverTimeByBorrowedDate(DateTime startDate, DateTime endDate);

        public List<BorrowTransactionModel> GetBorrowedTransactionsOverTimeByDueDate(DateTime startDate, DateTime endDate);

        public List<BorrowTransactionModel> GetBorrowTransactions();

        public List<BorrowTransactionModel> GetBorrowTransactionsByBookId(Guid bookId);
    }
}