using Microsoft.Extensions.Logging;
using ShoppingModule.API.Entities;

namespace ShoppingModule.API.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UnitOfWork> _logger;
        public UnitOfWork(ApplicationDbContext db, ILogger<UnitOfWork> logger)
        {
            _db = db;
            _logger = logger;
            productService = new ProductService(_db, _logger);
            categoryService = new CategoryService(_db, _logger);
            orderService = new OrderService(_db, _logger);
        }

        public IProductService productService { get; private set; }
        public ICategoryService categoryService { get; private set; }
        public IOrderService orderService { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
