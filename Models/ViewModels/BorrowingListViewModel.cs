namespace LibraryApp.Models.ViewModels
{
    public class BorrowingListViewModel
    {
        public Guid Id { get; set; }
        public string Surname { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public StatusEnum Status { get; set; }
        public string BookTitle { get; set; }  
    }
}
