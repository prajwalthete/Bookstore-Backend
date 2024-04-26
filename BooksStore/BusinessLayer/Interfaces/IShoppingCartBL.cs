using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IShoppingCartBL
    {
        public Task<ShoppingCart> CreateShoppingCart(int customerId);
        // Task<bool> DeleteShoppingCart(int cartId);
        public Task<bool> ClearShoppingCart(int cartId, int customerId);
    }
}
