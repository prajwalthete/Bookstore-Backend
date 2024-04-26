using ModelLayer.Models.OrderItem;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderItemRL
    {
        public Task<OrderItem> AddOrderItem(AddOrderItemModel addOrderItemModel);
        public Task<IEnumerable<OrderItem>> GetOrderItemsByOrderId(int orderId);
    }
}
