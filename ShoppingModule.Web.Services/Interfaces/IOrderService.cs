using ShoppingModule.Web.Entities;

namespace ShoppingModule.Web.Services
{
    public interface IOrderService
    {
        string SubmitPurchaseOrder(Order purchaseOrder);
    }
}
