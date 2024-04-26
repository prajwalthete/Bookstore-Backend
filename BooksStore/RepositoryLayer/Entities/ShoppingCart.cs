namespace RepositoryLayer.Entities
{
    public class ShoppingCart
    {
        public int cart_id { get; set; }        // Primary Key
        public int customer_id { get; set; }    // Foreign Key
    }
}
