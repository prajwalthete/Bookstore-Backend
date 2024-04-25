﻿using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models.Order;
using System.Security.Claims;

namespace BooksStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBL _orderBL;
        private readonly ILogger<UserController> _logger;

        public OrderController(IOrderBL orderBL, ILogger<UserController> logger)
        {
            _orderBL = orderBL;
            _logger = logger;
        }

        [Authorize(Roles = "customer")]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderModel placeOrderModel)
        {
            try
            {
                if (placeOrderModel == null)
                {
                    return BadRequest("Order details are missing.");
                }

                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int customerid = Convert.ToInt32(userIdClaim);
                await Console.Out.WriteLineAsync(customerid.ToString());

                var placedOrder = await _orderBL.PlaceOrder(placeOrderModel, customerid);

                // Log successful order placement
                _logger.LogInformation($"Order placed successfully with ID: {placedOrder.order_id}");

                var response = new
                {
                    Success = true,
                    Message = "Order placed successfully",
                    Data = placedOrder
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while placing the order");

                var errorResponse = new
                {
                    Success = false,
                    Message = "An error occurred while placing the order",
                    Error = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }
    }
}