using ModelLayer.Models.Book;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IBookBL
    {
        public Task<Book> AddBook(BookAddModel bookAddModel);
    }
}
