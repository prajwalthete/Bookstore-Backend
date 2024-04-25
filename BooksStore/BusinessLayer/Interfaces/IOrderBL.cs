﻿using ModelLayer.Models.Order;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IOrderBL
    {
        public Task<Order> PlaceOrder(PlaceOrderModel placeOrderModel, int customerid);
        public Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId);
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<Order> UpdateOrder(int orderId, PlaceOrderModel updatedOrder);
    }
}
