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
                string query = @" INSERT INTO Book (title, author, genre, price, ImagePath) VALUES (@Title, @Author, @Genre, @Price, @ImagePath);
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

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            try
            {

                string query = "SELECT * FROM Book";


                using (var connection = _context.CreateConnection())
                {
                    IEnumerable<Book> books = await connection.QueryAsync<Book>(query);
                    return books;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error occurred while retrieving all books", ex);
            }
        }

        public async Task<Book> UpdateBook(int bookId, UpdateBookModel updateBookModel)
        {
            try
            {
                string updateQuery = @"UPDATE Book SET 
                               title = @Title,
                               author = @Author,
                               genre = @Genre,
                               price = @Price,
                               ImagePath = @ImagePath
                               WHERE book_id = @BookId";


                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(updateQuery, new
                    {
                        updateBookModel.title,
                        updateBookModel.author,
                        updateBookModel.genre,
                        updateBookModel.price,
                        updateBookModel.ImagePath,
                        BookId = bookId
                    });

                    // Retrieve the updated book from the database
                    string selectQuery = "SELECT * FROM Book WHERE book_id = @BookId";
                    var updatedBook = await connection.QueryFirstOrDefaultAsync<Book>(selectQuery, new { BookId = bookId });

                    return updatedBook;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                throw new Exception("Error occurred while updating book", ex);
            }
        }
    }
}
