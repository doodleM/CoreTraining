using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingModule.Web.Entities;
using ShoppingModule.Web.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingModule.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var orders = new List<Order>();
            var sessionValue = HttpContext.Session.GetString("SearchedValue");
            var searchTerm = TempData["SearchTerm"] as string;

            if (string.IsNullOrEmpty(sessionValue))
            {
                orders = _orderService.GetAllOrders().ToList();
            }
            else
            {
                orders = JsonConvert.DeserializeObject<List<Order>>(sessionValue);
                HttpContext.Session.Remove("SearchedValue");
                TempData.Remove("CategoryId");
                TempData.Remove("SortValue");
            }

            if (orders.Count() > 0)
            {
                orders.FirstOrDefault().SearchTerm = searchTerm;
            }
            return View(orders.AsEnumerable());
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            Order createdOrder = new Order();
            string key = HttpContext.Session.Id;
            string storedValues = HttpContext.Session.GetString(key);
            if (!string.IsNullOrEmpty(storedValues))
            {
                var cartSession = JsonConvert.DeserializeObject<List<Cart>>(storedValues);
                if (cartSession != null)
                {
                    createdOrder.TotalPrice = cartSession.Sum(x => x.Price * x.Quantity);
                    createdOrder.Quantity = cartSession.Sum(x => x.Quantity);
                    createdOrder.ProductIds = string.Join("|", cartSession.Select(x => x.ProductId));
                }
            }
            return View(createdOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(Order purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                var orderId = _orderService.SubmitPurchaseOrder(purchaseOrder);
                if (!string.IsNullOrEmpty(orderId))
                {
                    string key = HttpContext.Session.Id;
                    HttpContext.Session.Remove(key);
                    TempData.Remove("CartQuantity");
                    return RedirectToAction("ThankYou", new { orderId = orderId });
                }
                else
                {
                    return View("Error");
                }
            }
            return View("Index", "Product");
        }

        [HttpGet]
        public IActionResult ThankYou(string orderId)
        {
            ViewData["OrderId"] = orderId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string SearchTerm)
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                var orders = _orderService.GetAllOrders();
                orders = orders.Where(x => x.OrderId.ToLower().StartsWith(SearchTerm.ToLower()));
                HttpContext.Session.SetString("SearchedValue", JsonConvert.SerializeObject(orders));
                TempData["SearchTerm"] = SearchTerm;
            }
            else
            {
                var orders = _orderService.GetAllOrders();
                HttpContext.Session.SetString("SearchedValue", JsonConvert.SerializeObject(orders));
            }
            return RedirectToAction("Index");
        }
    }
}
