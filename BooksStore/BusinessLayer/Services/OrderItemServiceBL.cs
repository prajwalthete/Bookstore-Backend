using BusinessLayer.Interfaces;
using ModelLayer.Models.OrderItem;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class OrderItemServiceBL : IOrderItemBL
    {
        private readonly IOrderItemRL _orderItemRL;

        public OrderItemServiceBL(IOrderItemRL orderItemRL)
        {
            _orderItemRL = orderItemRL;
        }

        public Task<OrderItem> AddOrderItem(AddOrderItemModel addOrderItemModel)
        {
            return _orderItemRL.AddOrderItem(addOrderItemModel);
        }
    }
}
