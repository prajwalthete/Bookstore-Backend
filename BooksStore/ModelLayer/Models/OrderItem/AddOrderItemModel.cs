namespace ModelLayer.Models.OrderItem
{
    public class AddOrderItemModel
    {
        //public int order_item_id { get; set; }  // Primary Key
        public int order_id { get; set; }       // Foreign Key
        public int book_id { get; set; }        // Foreign Key
        public int quantity { get; set; }
    }
}
