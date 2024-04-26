using ModelLayer.Models.OrderItem;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IOrderItemBL
    {
        public Task<OrderItem> AddOrderItem(AddOrderItemModel addOrderItemModel);
        public Task<IEnumerable<OrderItem>> GetOrderItemsByOrderId(int orderId);

    }
}
