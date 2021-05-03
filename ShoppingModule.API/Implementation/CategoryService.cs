using Microsoft.Extensions.Logging;
using ShoppingModule.API.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingModule.API.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger _logger;

        public CategoryService(ApplicationDbContext db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var entity = await _db.FindAsync<Category>(id);
            _db.Categories.Remove(entity);
            return true;
        }

        public async Task<List<Category>> GetAllCategories(string sortOrder)
        {
            var categories = _db.Categories.AsEnumerable();
            if (!string.IsNullOrEmpty(sortOrder) && string.Equals(sortOrder, "desc", System.StringComparison.OrdinalIgnoreCase))
            {
                return categories?.OrderByDescending(x => x.Name)?.ToList();
            }
            return categories?.OrderBy(x => x.Name)?.ToList();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var product = await _db.FindAsync<Category>(id);
            return product;
        }

        public async Task<bool> InsertCategory(Category product)
        {
            await _db.Categories.AddAsync(product);
            return true;
        }

        public async Task<bool> UpdateCategory(Category product)
        {
            _db.Categories.Update(product);
            return true;
        }
    }
}
