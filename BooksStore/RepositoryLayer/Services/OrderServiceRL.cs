using Dapper;
using ModelLayer.Models.Order;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class OrderServiceRL : IOrderRL
    {
        private readonly BookStoreContext _context;

        public OrderServiceRL(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<Order> PlaceOrder(PlaceOrderModel placeOrderModel, int customerId)
        {
            try
            {
                string query = @"INSERT INTO [Order] (customer_id, order_date, address) VALUES (@CustomerId, @OrderDate, @Address);
                               SELECT SCOPE_IDENTITY();"; // Retrieve the ID of the inserted order

                // Execute the query and retrieve the inserted order's ID
                using (var connection = _context.CreateConnection())
                {
                    int orderId = await connection.ExecuteScalarAsync<int>(query, new
                    {
                        CustomerId = customerId,
                        OrderDate = DateTime.Now, // Using current date/time for order_date
                        Address = placeOrderModel.address,
                    });

                    // Construct a new Order object with the generated order ID
                    Order placedOrder = new Order
                    {
                        order_id = orderId,
                        customer_id = customerId,
                        order_date = DateTime.Now, // Using current date/time for order_date
                        address = placeOrderModel.address
                    };

                    return placedOrder;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while placing the order", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId)
        {
            try
            {
                string query = "SELECT * FROM [Order] WHERE customer_id = @CustomerId";

                // Execute the query and retrieve the orders
                using (var connection = _context.CreateConnection())
                {
                    IEnumerable<Order> orders = await connection.QueryAsync<Order>(query, new { CustomerId = customerId });
                    return orders;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving orders by customer ID", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            try
            {
                string query = "SELECT * FROM [Order]";


                using (var connection = _context.CreateConnection())
                {
                    IEnumerable<Order> orders = await connection.QueryAsync<Order>(query);
                    return orders;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving all orders", ex);
            }
        }

    }
}
