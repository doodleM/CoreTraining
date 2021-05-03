using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingModule.Web.Interfaces;
using ShoppingModule.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingModule.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(ILogger<ProductController> logger, IProductService productService, ICategoryService categoryService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var products = new List<Product>();
            var sessionValue = HttpContext.Session.GetString("SearchedValue");
            var selectedValue = TempData["CategoryId"] as string;
            var selectedSortValue = TempData["SortValue"] as string ?? string.Empty;
            int.TryParse(selectedValue, out int selectedIndex);

            if (string.IsNullOrEmpty(sessionValue))
            {
                products = _productService.GetAllProducts("asc").ToList();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<Product>>(sessionValue);
                HttpContext.Session.Remove("SearchedValue");
                TempData.Remove("CategoryId");
                TempData.Remove("SortValue");
            }

            if (products.Count() > 0)
            {
                var categories = _categoryService.GetAllCategories("asc");
                products.FirstOrDefault().CategoryList = categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                    Selected = i.Id == selectedIndex
                });

                List<SelectListItem> sortValues = new List<SelectListItem>();
                sortValues.Add(new SelectListItem() { Text = "Ascending", Value = "asc" });
                sortValues.Add(new SelectListItem() { Text = "Descending", Value = "desc" });
                if (!string.IsNullOrEmpty(selectedSortValue))
                {
                    sortValues.Where(x => x.Value == selectedSortValue).FirstOrDefault().Selected = true;
                }
                products.FirstOrDefault().SortingList = sortValues;
            }
            return View(products.AsEnumerable());
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            var categories = _categoryService.GetAllCategories("asc");
            Product productVM = new Product();

            if (id == null)
            {
                productVM.CategoryList = categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(productVM);
            }
            else
            {
                productVM = _productService.GetProductById(id);
                productVM.CategoryList = categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductId != 0)
                {
                    _productService.UpdateProduct(product);
                }
                else
                {
                    _productService.CreateProduct(product);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string categoryId, string sortBy)
        {
            if (!string.IsNullOrEmpty(categoryId))
            {
                var products = _productService.GetProductByCategory(categoryId, sortBy);
                HttpContext.Session.SetString("SearchedValue", JsonConvert.SerializeObject(products));
                TempData["CategoryId"] = categoryId;
            }
            else
            {
                var products = _productService.GetAllProducts(sortBy);
                HttpContext.Session.SetString("SearchedValue", JsonConvert.SerializeObject(products));
            }
            TempData["SortValue"] = sortBy;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}
