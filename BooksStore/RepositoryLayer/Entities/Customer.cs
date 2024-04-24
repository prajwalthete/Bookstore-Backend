namespace RepositoryLayer.Entities
{
    public class Customer
    {
        public int customer_id { get; set; }    // Primary Key
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }
}
