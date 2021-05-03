using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingModule.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingModule.API.Implementation
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger _logger;

        public ProductService(ApplicationDbContext db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var entity = await _db.FindAsync<Product>(id);
            _db.Products.Remove(entity);
            return true;
        }

        public async Task<List<Product>> GetAllProducts(string sortOrder = "")
        {
            var products = _db.Products.Include(x => x.Category).Where(z => z.ExpiryDate > DateTime.Now).AsEnumerable();
            if (!string.IsNullOrEmpty(sortOrder) && string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase))
            {
                return products?.OrderByDescending(x => x.Name)?.ToList();
            }
            return products?.OrderBy(x => x.Name)?.ToList();
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = _db.Products.Include(x => x.Category).Where(x => x.ProductId == id && x.ExpiryDate > DateTime.Now).FirstOrDefault();
            return product;
        }

        public async Task<List<Product>> GetProductsByCategory(string category, string sortOrder = "")
        {
            int.TryParse(category, out int catId);
            var products = _db.Products.Include(x => x.Category).Where(x => x.CategoryId == catId && x.ExpiryDate > DateTime.Now).AsEnumerable();
            if (!string.IsNullOrEmpty(sortOrder) && string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase))
            {
                return products?.OrderByDescending(x => x.Name)?.ToList();
            }
            return products?.OrderBy(x => x.Name)?.ToList();
        }

        public async Task<bool> InsertProduct(Product product)
        {
            await _db.Products.AddAsync(product);
            return true;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _db.Products.Update(product);
            return true;
        }
    }
}
