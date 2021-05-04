using ShoppingModule.Web.Entities;
using System.Collections.Generic;

namespace ShoppingModule.Web.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        string SubmitPurchaseOrder(Order purchaseOrder);
    }
}
