using ShoppingModule.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingModule.API.Services
{
    public interface IOrderService
    {
        Task<string> PurchaseOrder(Order purchaseOrder);
        Task<List<Order>> GetAllOrders();
    }
}
