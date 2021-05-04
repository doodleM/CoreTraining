using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingModule.Web.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Search Term")]
        public string SearchTerm { get; set; }
    }
}
