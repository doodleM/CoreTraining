using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingModule.Web.Entities
{
    public class Order
    {
        public string ProductIds { get; set; }
        [Required]
        [DisplayName("Total Price")]
        public decimal TotalPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [DisplayName("Full Name")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [DisplayName("Email Address")]
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(20)]
        [Required]
        [DisplayName("Mobile Number")]
        public string Mobile { get; set; }
        [Required]
        [DisplayName("Shipping Address")]
        [MaxLength(200)]
        public string ShippingAddress { get; set; }
        [Required]
        [DisplayName("Billing Address")]
        [MaxLength(200)]
        public string BillingAddress { get; set; }
    }
}
