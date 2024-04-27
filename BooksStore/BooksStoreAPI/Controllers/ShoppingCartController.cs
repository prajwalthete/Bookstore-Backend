using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BooksStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartBL _shoppingCartBL;
        private readonly ILogger<ShoppingCartController> _logger;
        public ShoppingCartController(IShoppingCartBL shoppingCartBL, ILogger<ShoppingCartController> logger)
        {
            _shoppingCartBL = shoppingCartBL;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateShoppingCart()
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int customerId = Convert.ToInt32(userIdClaim);
                var shoppingCart = await _shoppingCartBL.CreateShoppingCart(customerId);
                if (shoppingCart != null)
                {
                    return Ok(new { success = true, message = $"ShoppingCart created", data = shoppingCart });
                }
                return BadRequest(new { success = false, message = "Failed to create shopping cart" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating shopping cart");
                return BadRequest(new { success = false, message = "An error occurred while creating shopping cart", error = ex.Message });
            }

        }


        [HttpDelete]
        [Route("{cartId}")]
        public async Task<ActionResult<bool>> ClearShoppingCart(int cartId)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int customerId = Convert.ToInt32(userIdClaim);
                bool cleared = await _shoppingCartBL.ClearShoppingCart(cartId, customerId);

                if (cleared)
                {
                    _logger.LogInformation($"Shopping cart {cartId} cleared for customer #{customerId}");
                    return Ok(new { success = true, message = "cart cleared Successfully" });
                }
                else
                {
                    _logger.LogWarning($"No shopping cart found with ID {cartId} for customer #{customerId}");
                    return NotFound(new { success = false, message = "No shopping cart found for the specified customer and ID." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error clearing shopping cart {cartId} for customer ");
                return BadRequest(new { success = false, message = "An error occurred while clearing the shopping cart.", error = ex.Message });

            }
        }


    }
}
