using ModelLayer.Models.Order;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderRL
    {
        public Task<Order> PlaceOrder(PlaceOrderModel placeOrderModel, int customerid);
        public Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId);

        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<Order> UpdateOrder(int orderId, PlaceOrderModel updatedOrder);

        public Task<bool> DeleteOrder(int orderId, int customerId);
    }

}
