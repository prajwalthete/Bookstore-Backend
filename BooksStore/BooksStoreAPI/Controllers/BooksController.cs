using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models.Book;
using System.Security.Claims;

namespace BooksStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookBL _bookBL;
        private readonly ILogger<UserController> _logger;

        public BooksController(ILogger<UserController> logger, IBookBL bookBL)
        {
            _logger = logger;
            _bookBL = bookBL;
        }



        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddBook(BookAddModel bookAddModel)
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                //if (!userRole.Equals("admin"))
                //{

                //    var errorResponse = new
                //    {
                //        Success = false,
                //        Message = "An error occurred while adding book Only Admin Can Add The Books",
                //    };

                //    return StatusCode(500, errorResponse);
                //}
                if (bookAddModel == null)
                {

                    return BadRequest("Book object is null");
                }

                var addedBook = await _bookBL.AddBook(bookAddModel);

                // Log successful addition of the book
                _logger.LogInformation($"Book '{addedBook.title}' added successfully.");

                var response = new
                {
                    Success = true,
                    Message = "Book added successfully",
                    Data = addedBook
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding book");

                var errorResponse = new
                {
                    Success = false,
                    Message = "An error occurred while adding book",
                    Error = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _bookBL.GetAllBooks();

                var response = new
                {
                    Success = true,
                    Message = "Books retrieved successfully",
                    Data = books
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving books");

                var errorResponse = new
                {
                    Success = false,
                    Message = "An error occurred while retrieving books",
                    Error = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] UpdateBookModel updateBookModel)
        {
            try
            {
                if (updateBookModel == null)
                {
                    return BadRequest("Book object is null");
                }

                var updatedBook = await _bookBL.UpdateBook(bookId, updateBookModel);

                _logger.LogInformation($"Book '{updatedBook.title}' updated successfully.");

                var response = new
                {
                    Success = true,
                    Message = "Book updated successfully",
                    Data = updatedBook
                };

                return Ok(response);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating book");

                var errorResponse = new
                {
                    Success = false,
                    Message = "An error occurred while updating book",
                    Error = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            try
            {
                var deleted = await _bookBL.DeleteBook(bookId);

                if (deleted)
                {

                    _logger.LogInformation($"Book with ID {bookId} deleted successfully.");

                    return Ok(new { Success = true, Message = "Book deleted successfully" });
                }
                else
                {
                    return NotFound(new { Success = false, Message = $"Book with ID {bookId} not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting book with ID {bookId}");

                return StatusCode(500, new { Success = false, Message = "An error occurred while deleting book", Error = ex.Message });
            }
        }


    }
}
