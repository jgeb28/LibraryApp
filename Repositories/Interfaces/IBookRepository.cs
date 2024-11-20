using LibraryApp.Controllers.dto;
using LibraryApp.Models;

namespace LibraryApp.Repositories.Interfaces
{
    public interface IBookRepository
    {
        public List<BookModel> GetAllBooks();
        public void AddNewBook(BookModel book);
        public void UpdateBook(Guid id, UpdateBookDto updateBookDto);

        public BookModel GetBookById(Guid id);

        public void DeleteBook(Guid id);
    }
}
