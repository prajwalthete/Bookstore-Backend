using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class ShoppingCartItemServiceBL : IShoppingCartItemBL
    {
        private readonly IShoppingCartItemRL _shoppingCartItemRL;

        public ShoppingCartItemServiceBL(IShoppingCartItemRL shoppingCartItemRL)
        {
            _shoppingCartItemRL = shoppingCartItemRL;
        }

        public Task<ShoppingCartItem> AddCartItem(int customerId, int bookId, int quantity)
        {
            return _shoppingCartItemRL.AddCartItem(customerId, bookId, quantity);
        }

        public Task<IEnumerable<ShoppingCartItem>> GetCartItems(int customerId)
        {
            return _shoppingCartItemRL.GetCartItems(customerId);
        }

        public Task<bool> RemoveCartItem(int cartItemId)
        {
            return _shoppingCartItemRL.RemoveCartItem(cartItemId);
        }

        public Task<bool> UpdateCartItemQuantity(int cartItemId, int newQuantity)
        {
            return _shoppingCartItemRL.UpdateCartItemQuantity(cartItemId, newQuantity);
        }
    }
}
