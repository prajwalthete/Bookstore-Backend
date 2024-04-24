using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.ExceptionHandler;
using System.Security.Claims;

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



        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint(string expectedUserEmail)
        {
            // Extract user Email claim from the token

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int UserId = Convert.ToInt32(userIdClaim);
            Console.WriteLine(UserId);

            var userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            Console.WriteLine(userEmailClaim);

            var userRole = User.FindFirstValue(ClaimTypes.Role);
            Console.WriteLine(userRole);

            if (userEmailClaim == null)
            {
                return Unauthorized("Invalid token");
            }

            // Compare the user email from the token with the expectedEmail
            if (!expectedUserEmail.Equals(userEmailClaim))
            {
                return Unauthorized("You are not authorized to access this resource.");
            }

            // This endpoint can only be accessed with a valid JWT token and the correct user ID
            return Ok("Welcome to the FundooNotes!");

        }


        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            try
            {

                string Token = await _customerBL.ForgetPassword(forgetPasswordModel);

                //HttpContext.Response.Headers.Add("Authorization", $"Bearer {Token}");

                if (Token != null)
                {
                    var response = new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Email sent successfully.",
                        // Data = Token

                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to send email.",
                        Data = null
                    };
                    return BadRequest(response);
                }
            }
            catch (NotFoundException ex)
            {
                var response = new ResponseModel<string>
                {

                    Success = false,
                    Message = $"Error sending email: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, response);
            }
            catch (EmailSendingException ex)
            {
                var response = new ResponseModel<string>
                {

                    Success = false,
                    Message = $"Error sending email: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = $"An unexpected error occurred: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, response);
            }
        }



        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordWithOTPModel resetPasswordModel)
        {
            try
            {
                bool isPasswordReset = await _customerBL.ResetPassword(resetPasswordModel);
                if (isPasswordReset)
                {
                    var response = new ResponseModel<bool>
                    {
                        Success = true,
                        Message = "Password reset successfully",
                        Data = isPasswordReset
                    };
                    return Ok(response);

                }
                return BadRequest();



            }
            catch (InvalidOTPException ex)
            {
                var response = new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false
                };

                return BadRequest(response);
            }
            catch (NotFoundException ex)
            {
                var response = new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false
                };

                return NotFound(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false
                };

                return StatusCode(500, response);
            }
        }





    }
}
