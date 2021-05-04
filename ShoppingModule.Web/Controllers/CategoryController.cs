using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingModule.Web.Entities;
using ShoppingModule.Web.Services;

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
            var categories = _categoryService.GetAllCategories("asc");
            return View(categories);
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
    }
}
