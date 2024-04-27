using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IShoppingCartItemRL
    {
        public Task<ShoppingCartItem> AddCartItem(int customerId, int bookId, int quantity);
        public Task<bool> RemoveCartItem(int cartItemId);
        public Task<bool> UpdateCartItemQuantity(int cartItemId, int newQuantity);
        public Task<IEnumerable<ShoppingCartItem>> GetCartItems(int customerId);
        // public Task<int> GetTotalCartQuantity(int cartId);
    }
}
