namespace RepositoryLayer.Entities
{
    public class ShoppingCartItem
    {
        public int cart_item_id { get; set; } // Primary Key
        public int cart_id { get; set; } // Foreign Key
        public int book_id { get; set; } // Foreign Key
        public int quantity { get; set; }
    }

}
