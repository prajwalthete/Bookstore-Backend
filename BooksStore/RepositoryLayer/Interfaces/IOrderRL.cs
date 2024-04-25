using ModelLayer.Models.Order;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderRL
    {
        public Task<Order> PlaceOrder(PlaceOrderModel placeOrderModel, int customerid);
    }
}
