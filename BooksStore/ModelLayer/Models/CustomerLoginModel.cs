using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models
{
    public class CustomerLoginModel
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
