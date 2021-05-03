using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingModule.Web.Interfaces;
using ShoppingModule.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingModule.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IProductService _productService;

        public CartController(ILogger<CartController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult InitializeCart(int id, string error ="")
        {
            var product = _productService.GetProductById(id);
            if (product != null)
            {
                Cart initializeCart = new Cart();
                initializeCart.ProductId = product.ProductId;
                initializeCart.Price = product.Price;
                initializeCart.Name = product.Name;
                initializeCart.Description = product.Description;
                initializeCart.error = error;
                return View(initializeCart);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult ClearCart()
        {
            string key = HttpContext.Session.Id;
            HttpContext.Session.Remove(key);
            TempData.Remove("CartQuantity");
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(Cart cart)
        {
            List<Cart> cartSession = new List<Cart>();
            if (ModelState.IsValid)
            {
                cart.TotalAmount = cart.Price * cart.Quantity;
                string key = HttpContext.Session.Id;
                string storedValues = HttpContext.Session.GetString(key);
                if (!string.IsNullOrEmpty(storedValues))
                {
                    cartSession = JsonConvert.DeserializeObject<List<Cart>>(storedValues);
                    if (cartSession.Any(x => x.ProductId == cart.ProductId))
                    {
                        var itemToUpdate = cartSession.FirstOrDefault(x => x.ProductId == cart.ProductId);
                        itemToUpdate.Quantity += cart.Quantity;
                        itemToUpdate.TotalAmount = cart.Price * itemToUpdate.Quantity;
                    }
                    else
                    {
                        cartSession.Add(cart);
                    }
                }
                else
                {
                    cartSession.Add(cart);
                }
                int totalSum = cartSession.Sum(x => x.Quantity);
                if (totalSum > 10)
                {
                    return RedirectToAction("InitializeCart", new { id = cart.ProductId, error = "Cart Quantity Exceeded!" });
                }
                else
                {
                    HttpContext.Session.SetString(key, JsonConvert.SerializeObject(cartSession));
                    TempData["CartQuantity"] = totalSum;
                    return RedirectToAction("Index", "Product");
                }
            }
            else
            {
                return RedirectToAction("Index", "Product");
            }
        }
    }
}
