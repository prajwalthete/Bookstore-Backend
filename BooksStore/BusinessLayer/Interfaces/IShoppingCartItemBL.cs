using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IShoppingCartItemBL
    {
        Task<ShoppingCartItem> AddCartItem(int cartId, int bookId, int quantity);
        Task<bool> RemoveCartItem(int cartItemId);
        Task<IEnumerable<ShoppingCartItem>> GetCartItems(int cartId);
        public Task<bool> UpdateCartItemQuantity(int cartItemId, int newQuantity);
    }
}
