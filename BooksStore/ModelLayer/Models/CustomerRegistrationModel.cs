using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models
{
    public class CustomerRegistrationModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string role { get; set; }
    }
}
