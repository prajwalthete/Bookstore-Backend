using Dapper;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class ShoppingCartItemServiceRL : IShoppingCartItemRL
    {
        private readonly BookStoreContext _context;

        public ShoppingCartItemServiceRL(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCartItem> AddCartItem(int cartId, int bookId, int quantity)
        {
            const string sql = @"
        INSERT INTO ShoppingCartItem (cart_id, book_id, quantity)
        OUTPUT INSERTED.*
        VALUES (@CartId, @BookId, @Quantity);";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new
                    {
                        CartId = cartId,
                        BookId = bookId,
                        Quantity = quantity
                    };

                    var cartItem = await connection.QuerySingleAsync<ShoppingCartItem>(sql, parameters);
                    return cartItem;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., logging)
                throw ex;
            }
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetCartItems(int cartId)
        {
            const string sql = @"
            SELECT *
            FROM ShoppingCartItem
            WHERE cart_id = @CartId;";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new { CartId = cartId };
                    var cartItems = await connection.QueryAsync<ShoppingCartItem>(sql, parameters);
                    return cartItems;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., logging)
                throw ex;
            }

        }

        public async Task<bool> RemoveCartItem(int cartItemId)
        {
            const string sql = @" DELETE FROM ShoppingCartItem WHERE cart_item_id = @CartItemId;";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new { CartItemId = cartItemId };
                    var rowsAffected = await connection.ExecuteAsync(sql, parameters);
                    return rowsAffected > 0; // Return true if at least one row was deleted
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., logging)
                throw ex;
            }
        }

        public async Task<bool> UpdateCartItemQuantity(int cartItemId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("New quantity must be positive."); // Handle invalid quantity
            }

            const string sql = @"
            UPDATE ShoppingCartItem
            SET quantity = @NewQuantity
            WHERE cart_item_id = @CartItemId;";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new { CartItemId = cartItemId, NewQuantity = newQuantity };
                    var rowsAffected = await connection.ExecuteAsync(sql, parameters);
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., logging)
                throw ex;
            }
        }
    }
}
