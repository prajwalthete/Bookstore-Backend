using Dapper;
using ModelLayer.Models.OrderItem;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class OrderItemServiceRL : IOrderItemRL
    {
        private readonly BookStoreContext _context;

        public OrderItemServiceRL(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> AddOrderItem(AddOrderItemModel addOrderItemModel)
        {
            try
            {
                // Define the SQL query to insert the order item and retrieve its ID
                string query = @"INSERT INTO OrderItem (order_id, book_id, quantity) 
                        VALUES (@OrderId, @BookId, @Quantity);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

                // Execute the query and retrieve the ID of the inserted order item
                using (var connection = _context.CreateConnection())
                {
                    int orderItemId = await connection.ExecuteScalarAsync<int>(query, new
                    {
                        OrderId = addOrderItemModel.order_id,
                        BookId = addOrderItemModel.book_id,
                        Quantity = addOrderItemModel.quantity
                    });

                    // Construct a new OrderItem object with the generated order item ID
                    OrderItem addedOrderItem = new OrderItem
                    {
                        order_item_id = orderItemId,
                        order_id = addOrderItemModel.order_id,
                        book_id = addOrderItemModel.book_id,
                        quantity = addOrderItemModel.quantity
                    };

                    return addedOrderItem;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                throw new Exception("Error occurred while adding the order item", ex);
            }
        }

        //Retrieves all order items for a specific order.
        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            try
            {
                string query = "SELECT * FROM OrderItem WHERE order_id = @OrderId";

                // Execute the query and retrieve the order items
                using (var connection = _context.CreateConnection())
                {
                    IEnumerable<OrderItem> orderItems = await connection.QueryAsync<OrderItem>(query, new { OrderId = orderId });
                    return orderItems;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving order items by order ID", ex);
            }
        }

        public async Task<OrderItem> UpdateOrderItem(int orderItemId, AddOrderItemModel updatedItem)
        {
            try
            {
                string query = @"UPDATE OrderItem  SET book_id = @BookId,quantity = @Quantity WHERE order_item_id = @OrderItemId";

                // Execute the update query
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new
                    {
                        BookId = updatedItem.book_id,
                        Quantity = updatedItem.quantity,
                        OrderItemId = orderItemId
                    });
                }

                // Retrieve the updated order item
                string selectQuery = "SELECT * FROM OrderItem WHERE order_item_id = @OrderItemId";
                using (var connection = _context.CreateConnection())
                {
                    var updatedOrderItem = await connection.QueryFirstOrDefaultAsync<OrderItem>(selectQuery, new { OrderItemId = orderItemId });
                    return updatedOrderItem;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the order item", ex);
            }
        }


        public async Task<bool> DeleteOrderItem(int orderItemId)
        {
            try
            {
                string query = "DELETE FROM OrderItem WHERE order_item_id = @OrderItemId";

                // Execute the delete query
                using (var connection = _context.CreateConnection())
                {
                    int rowsAffected = await connection.ExecuteAsync(query, new { OrderItemId = orderItemId });
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the order item", ex);
            }
        }



    }
}
