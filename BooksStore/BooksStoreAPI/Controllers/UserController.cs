using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.ExceptionHandler;

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
            catch (EmailAlreadyExistsException ex)
            {
                // Log the exception
                _logger.LogError(ex, $"Error occurred during customer registration: {ex.Message}");

                var errorResponse = new ResponseModel<string>
                {
                    Success = false,
                    Message = "Email already exists",
                    Data = ex.Message
                };
                return Conflict(errorResponse);
            }
            catch (Exception ex)
            {
                // Log other exceptions
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



        [HttpPost("login")]
        public async Task<IActionResult> Login(CustomerLoginModel userLogin)
        {
            try
            {
                string token = await _customerBL.Login(userLogin);

                // Log successful login
                _logger.LogInformation($"User with email '{userLogin.email}' logged in successfully.");


                var response = new ResponseModel<string>
                {
                    Message = "Login Successful",
                    Data = token
                };
                return Ok(response);
            }
            catch (InvalidLoginException ex)
            {
                // Log invalid login attempt
                _logger.LogWarning(ex, $"Invalid login attempt for email '{userLogin.email}'");


                return BadRequest("Invalid email or password");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unexpected error occurred during login");


                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
}
