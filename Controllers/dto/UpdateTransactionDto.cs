using LibraryApp.Models;

namespace LibraryApp.Controllers.dto
{
    public class UpdateTransactionDto
    {
        public Guid BorrowedBook { get; set; }

        public string Surname { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public StatusEnum Status { get; set; }
    }
}