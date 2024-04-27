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


        public async Task<Order> BuyNow(PlaceOrderModel placeOrderModel, int customerId)
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


        public async Task<Order> PlaceOrder(PlaceOrderModel placeOrderModel, int customerId)
        {
            try
            {
                string orderInsertQuery = @"INSERT INTO [Order] (customer_id, order_date, address) VALUES (@CustomerId, @OrderDate, @Address);
                    SELECT SCOPE_IDENTITY();"; // Retrieve the ID of the inserted order

                var connection = _context.CreateConnection();
                connection.Open(); // Open the connection before using it

                // Execute the query and retrieve the inserted order's ID
                using (var transaction = connection.BeginTransaction())
                {
                    int orderId = await connection.ExecuteScalarAsync<int>(orderInsertQuery, new
                    {
                        CustomerId = customerId,
                        OrderDate = DateTime.Now, // Using current date/time for order_date
                        Address = placeOrderModel.address
                    }, transaction); // Pass transaction as a separate parameter


                    // Step 2: Retrieve all items from the user's cart

                    var cartItemsQuery = @"SELECT * FROM ShoppingCartItem  WHERE Cart_Id = (SELECT Cart_Id FROM ShoppingCart WHERE customer_id = @UserId)";
                    var cartItems = (await connection.QueryAsync<ShoppingCartItem>(cartItemsQuery, new { UserId = customerId }, transaction)).ToList();

                    // Check if the cart is empty
                    if (!cartItems.Any()) //!false
                    {
                        throw new InvalidOperationException("The cart is empty.");
                    }


                    // Step 3: Bulk insert cart items into OrderItem table

                    var orderItemsInsertQuery = $@" INSERT INTO OrderItem (order_id, book_id, quantity)  VALUES ({orderId}, @book_id, @quantity)";

                    if (cartItems.Any()) //true
                    {
                        await connection.ExecuteAsync(orderItemsInsertQuery, cartItems, transaction);
                    }


                    // Step 4: Calculate total amount
                    //int totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

                    // Step 5: Update Order with total amount
                    // var updateOrderAmountQuery = "UPDATE [Order] SET Amount = @Amount WHERE Order_Id = @OrderId";
                    // await connection.ExecuteAsync(updateOrderAmountQuery, new { Amount = totalAmount, OrderId = orderId }, transaction);

                    // Step 6: Clear the cart
                    var clearCartQuery = "DELETE FROM ShoppingCartItem WHERE Cart_Id = (SELECT Cart_Id FROM ShoppingCart WHERE customer_id = @UserId)";
                    await connection.ExecuteAsync(clearCartQuery, new { UserId = customerId }, transaction);

                    // Commit transaction if all steps are successful
                    transaction.Commit();
                    //return true;



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


        public async Task<Order> UpdateOrder(int orderId, int customerId, PlaceOrderModel updatedOrderModel)
        {
            try
            {
                string query = @"UPDATE [Order] SET order_date = @OrderDate, address = @Address  WHERE order_id = @OrderId AND customer_id = @CustomerId";

                // Execute the update query
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new
                    {
                        OrderDate = DateTime.Now,
                        updatedOrderModel.address,
                        OrderId = orderId,
                        CustomerId = customerId
                    });
                }

                // Retrieve the updated order
                string selectQuery = "SELECT * FROM [Order] WHERE order_id = @OrderId AND customer_id = @CustomerId";
                using (var connection = _context.CreateConnection())
                {
                    var updatedOrder = await connection.QueryFirstOrDefaultAsync<Order>(selectQuery, new { OrderId = orderId, CustomerId = customerId });
                    return updatedOrder;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the order", ex);
            }
        }

        //public async Task<bool> DeleteOrder(int orderId, int customerId)
        //{
        //    try
        //    {
        //        string query = "DELETE FROM [Order] WHERE order_id = @OrderId AND customer_id = @CustomerId";


        //        using (var connection = _context.CreateConnection())
        //        {
        //            int rowsAffected = await connection.ExecuteAsync(query, new { OrderId = orderId, CustomerId = customerId });
        //            return rowsAffected > 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error occurred while deleting the order", ex);
        //    }
        //}

        public async Task<bool> DeleteOrder(int orderId, int customerId)
        {
            try
            {
                // Delete order items first
                string deleteItemsQuery = "DELETE FROM OrderItem WHERE order_id = @OrderId";

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(deleteItemsQuery, new { OrderId = orderId });

                    // Once order items are deleted, delete the order
                    string deleteOrderQuery = "DELETE FROM [Order] WHERE order_id = @OrderId AND customer_id = @CustomerId";

                    int rowsAffected = await connection.ExecuteAsync(deleteOrderQuery, new { OrderId = orderId, CustomerId = customerId });
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the order", ex);
            }
        }


    }
}
