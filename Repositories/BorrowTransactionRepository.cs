
using LibraryApp.Models;
using System.Text.Json;
using LibraryApp.Repositories.Interfaces;
using LibraryApp.Controllers.dto;

namespace LibraryApp.Repositories
{
    public class BorrowTransactionRepository : IBorrowTransactionRepository
    {
        private readonly string _filePath;

        public BorrowTransactionRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<BorrowTransactionModel> GetBorrowTransactions()
        {
            if (!File.Exists(_filePath))
            {
                return new List<BorrowTransactionModel>();
            }
            var jsonData = File.ReadAllText(_filePath);
            try
            {
                return JsonSerializer.Deserialize<List<BorrowTransactionModel>>(jsonData) ?? new List<BorrowTransactionModel>();
            }
            catch (Exception ex)
            {
                return new List<BorrowTransactionModel>();
            }
        }
        public List<BorrowTransactionModel> GetBorrowedTransactionsOverTimeByBorrowedDate(DateTime startDate, DateTime endDate)
        {
            var transactions = GetBorrowTransactions();

            return transactions.FindAll(transaction => transaction.BorrowDate >= startDate && transaction.BorrowDate <= endDate);
        }

        public List<BorrowTransactionModel> GetBorrowedTransactionsOverTimeByDueDate(DateTime startDate, DateTime endDate)
        {
            var transactions = GetBorrowTransactions();

            return transactions.FindAll(transaction => transaction.DueDate >= startDate && transaction.DueDate <= endDate);
        }

        public List<BorrowTransactionModel> GetBorrowTransactionsByBookId(Guid bookId)
        {
            var transactions = GetBorrowTransactions();

            return transactions.FindAll(transaction => transaction.BorrowedBook == bookId);
        }
        private void SaveToFile(List<BorrowTransactionModel> transactions)
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException();
            }

            var jsonData = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, jsonData);
        }

        public void CreateTransaction(BookModel book, string surname)
        {
            if (!book.isAvailable())
            {
                throw new InvalidOperationException("The book is not available");
            }
            var borrowDate = DateTime.Now.Date;
            var dueDate = borrowDate.AddDays(30);

            var newTransaction = new BorrowTransactionModel(book.Id, surname, borrowDate, dueDate, StatusEnum.Borrowed);

            var allTransactions = GetBorrowTransactions();

            allTransactions.Add(newTransaction);


            SaveToFile(allTransactions);
        }

        public BorrowTransactionModel GetTransactionById(Guid id)
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException();
            }
            var transactions = GetBorrowTransactions();
            return transactions.Find(transaction => transaction.Id == id);
        }

        public void DeleteTransaction(Guid transactionId)
        {
            if (GetTransactionById(transactionId) == null)
            {
                throw new KeyNotFoundException($"Book with ID {transactionId} not found");
            }

            var transactions = GetBorrowTransactions();
            transactions.RemoveAll(transaction => transaction.Id == transactionId);

            SaveToFile(transactions);
        }

        public void UpdateTransactionStatus(Guid transactionId, StatusEnum newStatus)
        {
            var transaction = GetTransactionById(transactionId);

            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with ID {transactionId} not found");
            }

            transaction.ChangeStatus(newStatus);

            var transactions = GetBorrowTransactions();

            var index = transactions.FindIndex(item => item.Id == transactionId);

            if (index >= 0)
            {
                transactions[index] = transaction;
            }

            SaveToFile(transactions);
        }

    }
}