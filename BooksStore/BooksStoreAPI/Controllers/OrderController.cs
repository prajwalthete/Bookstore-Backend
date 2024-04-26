using BusinessLayer.Interfaces;
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

        [Authorize]
        [HttpGet("GetOrderByCustomerId")]
        public async Task<IActionResult> GetOrdersByCustomerId()
        {
            try
            {

                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int customerId = Convert.ToInt32(userIdClaim);
                var orders = await _orderBL.GetOrdersByCustomerId(customerId);

                var response = new
                {
                    Success = true,
                    Message = "Orders retrieved successfully",
                    Data = orders
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving orders by customer ID");
                return StatusCode(500, "An error occurred while retrieving orders by customer ID");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderBL.GetAllOrders();

                _logger.LogInformation("Successfully retrieved all orders");

                var response = new
                {
                    Success = true,
                    Message = "All orders retrieved successfully",
                    Data = orders
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all orders");

                var errorResponse = new
                {
                    Success = false,
                    Message = "An error occurred while retrieving orders",
                    Error = ex.Message
                };
                return StatusCode(500, errorResponse);
            }
        }

        [Authorize]
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, PlaceOrderModel updatedOrderModel)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int customerId = Convert.ToInt32(userIdClaim);
                var updatedOrder = await _orderBL.UpdateOrder(orderId, customerId, updatedOrderModel);
                if (updatedOrder != null)
                {
                    _logger.LogInformation($"Order with ID {orderId} updated successfully.");
                    return Ok(new
                    {
                        Success = true,
                        Message = "Order updated successfully",
                        Data = updatedOrder
                    });
                }
                else
                {
                    _logger.LogError($"Failed to update order with ID {orderId}. Order not found.");
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Order with ID {orderId} not found"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating order with ID {orderId}.");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while updating the order",
                    Error = ex.Message
                });
            }
        }


        [Authorize]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int customerId = Convert.ToInt32(userIdClaim);
                bool isDeleted = await _orderBL.DeleteOrder(orderId, customerId);
                if (isDeleted)
                {
                    _logger.LogInformation($"Order with ID {orderId} deleted successfully.");
                    var response = new
                    {
                        Success = true,
                        Message = $"Order with ID {orderId} deleted successfully.",
                        Data = true
                    };
                    return Ok(response);
                }
                else
                {
                    _logger.LogWarning($"Order with ID {orderId} not found or could not be deleted.");
                    var response = new
                    {
                        Success = false,
                        Message = $"Order with ID {orderId} not found or could not be deleted."
                    };
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting order with ID {orderId}: {ex.Message}");
                var response = new
                {
                    Success = false,
                    Message = $"An error occurred while deleting order with ID {orderId}."
                };
                return StatusCode(500, response);
            }
        }

    }
}
