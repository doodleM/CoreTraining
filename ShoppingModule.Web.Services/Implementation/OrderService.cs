using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingModule.Web.Entities;
using System.Collections.Generic;

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

        public IEnumerable<Order> GetAllOrders()
        {
            string orderListUrl = string.Format(_configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:OrderListingUrl"]);
            var response = _serviceContract.GetAsync<Response<List<Order>>>(orderListUrl)?.Result;
            return response?.Data;
        }

        public string SubmitPurchaseOrder(Order purchaseOrder)
        {
            string purchaseOrderUrl = _configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:PurchaseOrderUrl"];
            var response = _serviceContract.PostAsync<Response<string>>(purchaseOrderUrl, JsonConvert.SerializeObject(purchaseOrder))?.Result;
            return response.Data;
        }
    }
}
