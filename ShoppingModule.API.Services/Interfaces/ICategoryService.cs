using ShoppingModule.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingModule.API.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories(string sortOrder);
        Task<bool> InsertCategory(Category product);
        Task<bool> UpdateCategory(Category product);
        Task<bool> DeleteCategory(int id);
        Task<Category> GetCategoryById(int id);
    }
}
