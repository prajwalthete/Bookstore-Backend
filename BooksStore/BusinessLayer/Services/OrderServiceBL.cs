using BusinessLayer.Interfaces;
using ModelLayer.Models.Order;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class OrderServiceBL : IOrderBL
    {
        private readonly IOrderRL _orderRL;

        public OrderServiceBL(IOrderRL orderRL)
        {
            _orderRL = orderRL;
        }

        public Task<bool> DeleteOrder(int orderId, int customerId)
        {
            return _orderRL.DeleteOrder(orderId, customerId);
        }

        public Task<IEnumerable<Order>> GetAllOrders()
        {
            return _orderRL.GetAllOrders();
        }

        public Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId)
        {
            return _orderRL.GetOrdersByCustomerId(customerId);
        }

        public Task<Order> PlaceOrder(PlaceOrderModel placeOrderModel, int customerid)
        {
            return _orderRL.PlaceOrder(placeOrderModel, customerid);
        }

        public Task<Order> UpdateOrder(int orderId, int customerId, PlaceOrderModel updatedOrder)
        {
            return _orderRL.UpdateOrder(orderId, customerId, updatedOrder);
        }
    }
}
