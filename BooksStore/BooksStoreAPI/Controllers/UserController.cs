using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;

namespace BooksStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICustomerBL _customerBL;
        private readonly ILogger<UserController> _logger;

        public UserController(ICustomerBL customerBL, ILogger<UserController> logger)
        {
            _customerBL = customerBL;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CustomerRegistrationModel customerRegistrationModel)
        {
            try
            {
                bool registrationResult = await _customerBL.Register(customerRegistrationModel);
                if (registrationResult)
                {
                    var response = new ResponseModel<string>
                    {
                        Success = true,
                        Message = "User Registration Successful"
                    };
                    return Ok(response);
                }

                return BadRequest("Invalid input");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred during customer registration");

                var errorResponse = new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred during registration",
                    Data = ex.Message
                };
                return BadRequest(errorResponse);
            }
        }
    }
}
