using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{


    public class BorrowTransactionModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid BorrowedBook { get; set; }

        public string Surname { get; set; }

        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public StatusEnum Status { get; set; }

        public BorrowTransactionModel(Guid borrowedBook, string surname, DateTime borrowDate, DateTime dueDate, StatusEnum status)
        {
            BorrowedBook = borrowedBook;
            Surname = surname;
            BorrowDate = borrowDate;
            DueDate = dueDate;
            Status = status;
            ReturnDate = null;
        }

        public void ChangeStatus(StatusEnum status)
        {
            Status = status;

            if (Status == StatusEnum.Returned)
            {
                ReturnDate = DateTime.Now.Date;
            }
        }


    }
    public enum StatusEnum
    {
        Borrowed,
        Returned,
        Overdue
    }

}
