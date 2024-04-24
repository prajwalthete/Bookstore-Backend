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

        public UserController(ICustomerBL customerBL)
        {
            _customerBL = customerBL;
        }
        [HttpPost]
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

                return BadRequest("invalid input");

            }
            catch (Exception ex)
            {

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
