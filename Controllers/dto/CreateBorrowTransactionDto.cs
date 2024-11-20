using LibraryApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Controllers.dto
{
    public class CreateBorrowTransactionDto
    {

        [Required(ErrorMessage = "The BorrowedBook field is required.")]
        public Guid? BorrowedBook { get; set; }

        [Required(ErrorMessage = "The Surname field is required.")]
        public string Surname { get; set; }
    }
}