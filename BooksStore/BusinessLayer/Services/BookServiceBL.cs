using BusinessLayer.Interfaces;
using ModelLayer.Models.Book;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class BookServiceBL : IBookBL
    {
        private readonly IBookRL _bookRL;

        public BookServiceBL(IBookRL bookRL)
        {
            _bookRL = bookRL;
        }

        public Task<Book> AddBook(BookAddModel bookAddModel)
        {
            return _bookRL.AddBook(bookAddModel);
        }

        public Task<IEnumerable<Book>> GetAllBooks()
        {
            return _bookRL.GetAllBooks();
        }
    }
}
