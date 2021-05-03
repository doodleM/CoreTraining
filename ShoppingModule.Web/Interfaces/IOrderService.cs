using ShoppingModule.Web.Models;

namespace ShoppingModule.Web.Interfaces
{
    public interface IOrderService
    {
        string SubmitPurchaseOrder(Order purchaseOrder);
    }
}
