namespace RepositoryLayer.Entities
{
    public class Order
    {

        public int order_id { get; set; }       // Primary Key
        public int customer_id { get; set; }    // Foreign Key
        public DateTime order_date { get; set; }
        public string address { get; set; }

    }
}
