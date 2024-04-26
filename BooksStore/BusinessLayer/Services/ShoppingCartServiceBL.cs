using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class ShoppingCartServiceBL : IShoppingCartBL
    {
        private readonly IShoppingCartRL _shoppingCartRL;

        public ShoppingCartServiceBL(IShoppingCartRL shoppingCartRL)
        {
            _shoppingCartRL = shoppingCartRL;
        }

        public Task<bool> ClearShoppingCart(int cartId, int customerId)
        {
            return _shoppingCartRL.ClearShoppingCart(cartId, customerId);
        }

        public Task<ShoppingCart> CreateShoppingCart(int customerId)
        {
            return _shoppingCartRL.CreateShoppingCart(customerId);
        }
    }
}
