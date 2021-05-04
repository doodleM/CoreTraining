using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingModule.Web.Entities
{
    public class Product
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Length must be under 50")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Length must be under 200")]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string ProductCategory { get; set; }

        [DisplayName("Sub-Category")]
        public string SubCategory { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public DateTime ExpiryDate { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> SortingList { get; set; }
    }
}
