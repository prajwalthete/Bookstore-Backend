namespace RepositoryLayer.Entities
{
    public class Book
    {

        public int book_id { get; set; }    // Primary Key
        public string title { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public decimal price { get; set; }
    }

}
