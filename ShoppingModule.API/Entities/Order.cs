using System.ComponentModel.DataAnnotations;

namespace ShoppingModule.API
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; }
        public string ProductIds { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(20)]
        [Required]
        public string Mobile { get; set; }
        [Required]
        [MaxLength(200)]
        public string ShippingAddress { get; set; }
        [Required]
        [MaxLength(200)]
        public string BillingAddress { get; set; }
    }
}
