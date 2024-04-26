using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models.OrderItem;

namespace BooksStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemBL _orderItemBL;
        private readonly ILogger<OrderItemController> _logger;

        public OrderItemController(ILogger<OrderItemController> logger, IOrderItemBL orderItemBL)
        {
            _logger = logger;
            _orderItemBL = orderItemBL;
        }


        [Authorize]
        [HttpPost]
        [Route("AddOrderItem")]
        public async Task<IActionResult> AddOrderItem([FromBody] AddOrderItemModel addOrderItemModel)
        {
            try
            {
                var addedOrderItem = await _orderItemBL.AddOrderItem(addOrderItemModel);

                _logger.LogInformation($"Order item with ID {addedOrderItem.order_item_id} added successfully.");

                return Ok(new
                {
                    Success = true,
                    Message = "Order item added successfully",
                    Data = addedOrderItem
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the order item");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while adding the order item",
                    Error = ex.Message
                });
            }
        }


        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderItemsByOrderId(int orderId)
        {
            try
            {
                var orderItems = await _orderItemBL.GetOrderItemsByOrderId(orderId);
                _logger.LogInformation($"Retrieved order items for order ID: {orderId}");
                return Ok(new { success = true, message = "Order items retrieved successfully", data = orderItems });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving order items for order ID: {orderId}", ex);
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
    }
}
