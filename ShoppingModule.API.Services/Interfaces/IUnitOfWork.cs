using System;

namespace ShoppingModule.API.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IProductService productService { get; }
        ICategoryService categoryService { get; }
        IOrderService orderService { get; }
        void Save();
    }
}
