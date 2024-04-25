using Dapper;
using ModelLayer.Models.Book;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class BookServiceRL : IBookRL
    {
        private readonly BookStoreContext _context;

        public BookServiceRL(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<Book> AddBook(BookAddModel bookAddModel)
        {
            try
            {
                string query = @" INSERT INTO Book (title, author, genre, price) VALUES (@Title, @Author, @Genre, @Price);
                                  SELECT SCOPE_IDENTITY();"; // Retrieve the ID of the inserted book

                // Execute the query and retrieve the inserted book's ID
                using (var connection = _context.CreateConnection())
                {
                    int bookId = await connection.ExecuteScalarAsync<int>(query, bookAddModel);

                    // Retrieve the complete book entity with the ID
                    Book addedBook = await connection.QueryFirstOrDefaultAsync<Book>("SELECT * FROM Book WHERE book_id = @BookId", new { BookId = bookId });

                    return addedBook;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                throw new Exception("Error occurred while adding book", ex);
            }
        }

    }
}
