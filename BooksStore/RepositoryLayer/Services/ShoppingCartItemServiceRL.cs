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


        /*
        public async Task<ShoppingCartItem> AddCartItem(int customerId, int bookId, int quantity)
        {

            const string sql = @" DECLARE @CartId INT;

                                SELECT @CartId = cart_id
                                FROM ShoppingCart
                                WHERE customer_id = @CustomerId;


                                INSERT INTO ShoppingCartItem (cart_id, book_id, quantity)
                                OUTPUT INSERTED.* VALUES (@CartId, @BookId, @Quantity);";

            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var parameters = new { CustomerId = customerId, BookId = bookId, Quantity = quantity };
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
        */

        public async Task<ShoppingCartItem> AddCartItem(int customerId, int bookId, int quantity)
        {
            const string sqlGetCartId = @" SELECT cart_id FROM ShoppingCart WHERE customer_id = @CustomerId;";

            const string sqlInsertCartItem = @" INSERT INTO ShoppingCartItem (cart_id, book_id, quantity) OUTPUT INSERTED.* VALUES (@CartId, @BookId, @Quantity);";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    // Check if cart exists for the customer
                    var cartId = await connection.QuerySingleOrDefaultAsync<int>(sqlGetCartId, new { CustomerId = customerId });

                    if (cartId == default) // Check for default value (usually 0 for int)
                    {
                        // Create a new cart if it doesn't exist
                        cartId = await CreateCart(customerId);
                    }

                    // Insert the cart item using the retrieved or created cart ID
                    var cartItem = await connection.QuerySingleAsync<ShoppingCartItem>(sqlInsertCartItem, new { CartId = cartId, BookId = bookId, Quantity = quantity });
                    return cartItem;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task<int> CreateCart(int customerId)
        {
            const string sqlCreateCart = @" INSERT INTO ShoppingCart (customer_id) OUTPUT INSERTED.cart_id VALUES (@CustomerId);";


            using (var connection = _context.CreateConnection())
            {
                var parameters = new { CustomerId = customerId };
                var cartId = await connection.QuerySingleAsync<int>(sqlCreateCart, parameters);
                return cartId;
            }
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetCartItems(int customerId)
        {
            //const string sql = @"
            //SELECT *
            //FROM ShoppingCartItem
            //WHERE cart_id = @CartId;";


            const string sql = @"SELECT * FROM ShoppingCartItem  WHERE Cart_Id = (SELECT Cart_Id FROM ShoppingCart WHERE customer_id = @UserId)";


            try
            {
                using (var connection = _context.CreateConnection())
                {
                    //var parameters = new { CartId = customerId };
                    var cartItems = await connection.QueryAsync<ShoppingCartItem>(sql, new { UserId = customerId });
                    return cartItems.ToList();
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
