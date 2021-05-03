using ShoppingModule.Web.Models;
using System.Collections.Generic;

namespace ShoppingModule.Web.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories(string sortBy);
        bool CreateCategory(Category product);
        bool UpdateCategory(Category product);
        bool DeleteCategory(int id);
        Category GetCategoryById(int? id);
    }
}
