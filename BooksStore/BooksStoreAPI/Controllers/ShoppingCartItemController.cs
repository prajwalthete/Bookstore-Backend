﻿using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BooksStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartItemController : ControllerBase
    {
        private readonly IShoppingCartItemBL _shoppingCartItemBL;

        public ShoppingCartItemController(IShoppingCartItemBL shoppingCartItemBL)
        {
            _shoppingCartItemBL = shoppingCartItemBL;
        }

        [Authorize]
        [HttpPost("AddItemToCart")]
        public async Task<IActionResult> AddCartItem(int bookId, int quantity)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int customerId = Convert.ToInt32(userIdClaim);

                var cartItemId = await _shoppingCartItemBL.AddCartItem(customerId, bookId, quantity);

                return Ok(new { success = true, message = "Cart Items Added Successfully", Data = cartItemId });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = "Shopping cart item not found." });

            }
        }


        [Authorize]
        [HttpDelete]
        [Route("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            try
            {
                var isRemoved = await _shoppingCartItemBL.RemoveCartItem(cartItemId);
                if (isRemoved)
                {
                    return Ok(new { success = true, message = "Cart Items Removed Successfully" });
                }
                else
                {
                    return NotFound(new { success = false, message = "Shopping cart item not found." });
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., logging)
                return StatusCode(500, "An error occurred while removing the item from the cart.");
            }
        }


        [Authorize]
        [HttpGet]
        //  [Route("get/{cartId}")]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int customerId = Convert.ToInt32(userIdClaim);

                var cartItems = await _shoppingCartItemBL.GetCartItems(customerId);
                if (cartItems == null || !cartItems.Any())
                {
                    return NotFound(new { success = false, message = "No cart items found." });
                }
                return Ok(new { success = true, message = "Cart Items Retrieved Successfully", data = cartItems }); // 200 OK with data
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., logging)
                return StatusCode(500, "An error occurred while retrieving cart items.");
            }
        }


        [Authorize]
        [HttpPut]
        [Route("update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(int cartItemId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                return BadRequest(new { success = false, message = "New quantity must be positive." }); // 400 Bad Request for invalid input
            }

            try
            {
                var isUpdated = await _shoppingCartItemBL.UpdateCartItemQuantity(cartItemId, newQuantity);
                if (isUpdated)
                {
                    return Ok(new { success = true, message = "Cart Item Quantity Updated Successfully" });
                }
                else
                {
                    return NotFound(new { success = false, message = "Cart item not found." });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400 Bad Request for specific validation errors
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { success = false, message = "An error occurred while updating the cart item quantity." });
            }
        }
    }
}
