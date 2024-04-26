using Dapper;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class ShoppingCartServiceRL : IShoppingCartRL
    {
        private readonly BookStoreContext _context;

        public ShoppingCartServiceRL(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<bool> ClearShoppingCart(int cartId, int customerId)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "DELETE FROM ShoppingCart WHERE cart_id = @cartId AND cart_id IN (SELECT cart_id FROM ShoppingCart WHERE customer_id = @customerId);";
                    var rowsAffected = await connection.ExecuteAsync(sql, new { cartId = cartId, customerId = customerId });
                    return rowsAffected > 0; // Return true if at least one row was deleted
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return false
                throw;
            }
        }


        public async Task<ShoppingCart> CreateShoppingCart(int customerId)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    // Insert the new shopping cart
                    var insertSql = "INSERT INTO ShoppingCart (customer_id) VALUES (@CustomerId);";
                    await connection.ExecuteAsync(insertSql, new { CustomerId = customerId });
                }

                // Retrieve the complete entity of the newly created cart using a new connection
                using (var connection = _context.CreateConnection())
                {
                    var selectSql = "SELECT * FROM ShoppingCart WHERE customer_id = @customerId;";
                    var shoppingCart = await connection.QueryFirstOrDefaultAsync<ShoppingCart>(selectSql, new { customerId = customerId });

                    if (shoppingCart != null)
                    {
                        // Return the complete entity of the newly created cart
                        return shoppingCart;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the newly created shopping cart.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return a default value or throw them to the caller
                throw;
            }
        }



    }
}
