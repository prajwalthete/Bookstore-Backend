using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models.Book
{
    public class BookAddModel
    {
        [Required]
        public string title { get; set; }

        public string author { get; set; }
        public string genre { get; set; }
        public decimal price { get; set; }
        public string ImagePath { get; set; }
    }
}
