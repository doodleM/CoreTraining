using System.ComponentModel.DataAnnotations;

namespace ShoppingModule.Web.Models
{
    public class Cart
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Length must be under 50")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Length must be under 200")]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Value should be between 1 & 10")]
        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }
        public string error { get; set; }
    }
}
