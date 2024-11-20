using LibraryApp.Controllers.dto;
using LibraryApp.Models;
using LibraryApp.Repositories.Interfaces;
using System.Text.Json;

namespace LibraryApp.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _filePath;

        public BookRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<BookModel> GetAllBooks()
        {
            if (!File.Exists(_filePath))
            {
                return new List<BookModel>();
            }
            var jsonData = File.ReadAllText(_filePath);
            try
            {
                return JsonSerializer.Deserialize<List<BookModel>>(jsonData) ?? new List<BookModel>();
            }
            catch (Exception ex)
            {
                return new List<BookModel>();
            }
        }

        public void AddNewBook(BookModel book)
        {
            var books = GetAllBooks();
            if (isDuplicate(book, books))
            {
                throw new InvalidOperationException($"Book with the same ID: {book.Id} already exists.");
            }
            books.Add(book);

            SaveToFile(books);
        }

        public BookModel GetBookById(Guid id)
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException();
            }
            var books = GetAllBooks();

            return books.Find(book => book.Id == id);
        }




        private void SaveToFile(List<BookModel> books)
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException();
            }

            var jsonData = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, jsonData);
        }


        public void UpdateBook(Guid id, UpdateBookDto updateBookDto)
        {
            var book = GetBookById(id);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found");
            }

            var books = GetAllBooks();

            book.updateBookModel(updateBookDto);

            var index = books.FindIndex(book => book.Id == id);

            if (index >= 0)
            {
                books[index] = book;
            }

            SaveToFile(books);
        }

        public void DeleteBook(Guid id)
        {
            if (GetBookById(id) == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found");
            }

            var books = GetAllBooks();
            books.RemoveAll(book => book.Id == id);

            SaveToFile(books);
        }


        private bool isDuplicate(BookModel newBook, List<BookModel> books)
        {
            return books.Any(book => book.Id == newBook.Id);
        }
    }
}
