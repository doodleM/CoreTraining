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
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = new List<Category>();
            var sessionValue = HttpContext.Session.GetString("SearchedValue");
            var searchedValue = TempData["SearchTerm"] as string;

            if (string.IsNullOrEmpty(sessionValue))
            {
                categories = _categoryService.GetAllCategories("asc")?.ToList();
            }
            else
            {
                categories = JsonConvert.DeserializeObject<List<Category>>(sessionValue);
                HttpContext.Session.Remove("SearchedValue");
                TempData.Remove("SearchTerm");
            }

            if (categories.Count() > 0)
            {
                categories.FirstOrDefault().SearchTerm = searchedValue;
            }
            return View(categories.AsEnumerable());
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            Category categoryVM = new Category();

            if (id != null)
            {
                categoryVM = _categoryService.GetCategoryById(id);
            }
            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id != 0)
                {
                    _categoryService.UpdateCategory(category);
                }
                else
                {
                    _categoryService.CreateCategory(category);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _categoryService.DeleteCategory(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string SearchTerm)
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                var categories = _categoryService.GetAllCategories("asc");
                categories = categories.Where(x => x.Name.ToLower().StartsWith(SearchTerm.ToLower()));
                HttpContext.Session.SetString("SearchedValue", JsonConvert.SerializeObject(categories));
                TempData["SearchTerm"] = SearchTerm;
            }
            else
            {
                var categories = _categoryService.GetAllCategories("asc");
                HttpContext.Session.SetString("SearchedValue", JsonConvert.SerializeObject(categories));
            }
            return RedirectToAction("Index");
        }
    }
}
