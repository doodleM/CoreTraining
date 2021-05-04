using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingModule.Web.Entities;

namespace ShoppingModule.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IServicesContract _serviceContract;
        private readonly IConfiguration _configuration;
        public OrderService(IServicesContract serviceContract, IConfiguration configuration)
        {
            _serviceContract = serviceContract;
            _configuration = configuration;
        }

        public string SubmitPurchaseOrder(Order purchaseOrder)
        {
            string purchaseOrderUrl = _configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:PurchaseOrderUrl"];
            var response = _serviceContract.PostAsync<Response<string>>(purchaseOrderUrl, JsonConvert.SerializeObject(purchaseOrder))?.Result;
            return response.Data;
        }
    }
}
