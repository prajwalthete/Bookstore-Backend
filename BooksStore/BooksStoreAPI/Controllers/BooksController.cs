using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models.Book;

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


        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookAddModel bookAddModel)
        {
            try
            {
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

    }
}
