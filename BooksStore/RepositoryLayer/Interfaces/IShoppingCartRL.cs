using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IShoppingCartRL
    {
        public Task<ShoppingCart> CreateShoppingCart(int customerId);
        // Task<bool> DeleteShoppingCart(int cartId);
        public Task<bool> ClearShoppingCart(int cartId, int customerId);


    }
}
