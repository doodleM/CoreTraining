using System;

namespace ShoppingModule.API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductService productService { get; }
        ICategoryService categoryService { get; }
        IOrderService orderService { get; }
        void Save();
    }
}
