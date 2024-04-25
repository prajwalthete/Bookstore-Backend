using ModelLayer.Models.Book;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IBookRL
    {
        public Task<Book> AddBook(BookAddModel bookAddModel);
        public Task<Book> GetBookById(int bookId);
        public Task<IEnumerable<Book>> GetAllBooks();
        public Task<Book> UpdateBook(int bookId, UpdateBookModel updateBookModel);
        public Task<bool> DeleteBook(int bookId);
    }
}
