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
    }
}
