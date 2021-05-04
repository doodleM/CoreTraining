using ShoppingModule.Web.Entities;
using System.Collections.Generic;

namespace ShoppingModule.Web.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts(string sortBy);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(int id);
        Product GetProductById(int? id);
        IEnumerable<Product> GetProductByCategory(string category, string sortBy);
    }
}
