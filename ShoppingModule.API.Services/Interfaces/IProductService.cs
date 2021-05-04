using ShoppingModule.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingModule.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts(string sortOrder = "");
        Task<bool> InsertProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<Product> GetProductById(int id);
        Task<List<Product>> GetProductsByCategory(string category, string sortOrder = "");
    }
}
