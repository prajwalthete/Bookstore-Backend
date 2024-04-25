using System.Text.Json.Serialization;

namespace ModelLayer.Models.Book
{
    public class UpdateBookModel
    {
        [JsonIgnore]
        public int book_id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public decimal price { get; set; }
        public string ImagePath { get; set; }
    }
}
