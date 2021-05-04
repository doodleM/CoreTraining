using Microsoft.Extensions.Logging;
using ShoppingModule.API.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingModule.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger _logger;

        public OrderService(ApplicationDbContext db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<string> PurchaseOrder(Order purchaseOrder)
        {
            purchaseOrder.OrderId = RandomString(5);
            await _db.Orders.AddAsync(purchaseOrder);
            return purchaseOrder.OrderId;
        }

        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
