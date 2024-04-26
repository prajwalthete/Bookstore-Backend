using ModelLayer.Models.OrderItem;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IOrderItemBL
    {
        public Task<OrderItem> AddOrderItem(AddOrderItemModel addOrderItemModel);
        public Task<IEnumerable<OrderItem>> GetOrderItemsByOrderId(int orderId);
        public Task<OrderItem> UpdateOrderItem(int orderItemId, AddOrderItemModel updatedOrderItem);//Updates an existing order item with new information.
        public Task<bool> DeleteOrderItem(int orderItemId); //Deletes an order item from the database by its identifier.

    }
}
