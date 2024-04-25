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

        public Task<Order> PlaceOrder(PlaceOrderModel placeOrderModel, int customerid)
        {
            return _orderRL.PlaceOrder(placeOrderModel, customerid);
        }
    }
}
