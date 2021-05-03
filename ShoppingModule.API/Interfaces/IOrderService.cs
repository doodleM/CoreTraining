using System.Threading.Tasks;

namespace ShoppingModule.API.Interfaces
{
    public interface IOrderService
    {
        Task<string> PurchaseOrder(Order purchaseOrder);
    }
}
