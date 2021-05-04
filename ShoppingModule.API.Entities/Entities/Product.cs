using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingModule.API.Entities
{
    public class Product
    {
        [Key]
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "Length must be under 50")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Length must be under 200")]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public string SubCategory { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
