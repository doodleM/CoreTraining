using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingModule.API.Entities;
using ShoppingModule.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingModule.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(ILogger<CategoryController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> InsertCategory(Category entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.categoryService.InsertCategory(entity);
                    _unitOfWork.Save();
                    return Ok(new Response<bool>
                    {
                        Success = result,
                        Data = result,
                        Code = 200
                    });
                }
                else
                {
                    return Ok(new Response<string>
                    {
                        Success = false,
                        Code = 400,
                        Data = "Request body is not in correct format."
                    });
                }
            }
            catch(Exception ex)
            {
                return Ok(new Response<string>
                {
                    Success = false,
                    Code = 500,
                    Data = (ex.Message + ex.InnerException)
                });
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateCategory(Category entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.categoryService.UpdateCategory(entity);
                    _unitOfWork.Save();
                    return Ok(new Response<bool>
                    {
                        Success = result,
                        Data = result,
                        Code = 200
                    });
                }
                else
                {
                    return Ok(new Response<string>
                    {
                        Success = false,
                        Code = 400,
                        Data = "Request body is not in correct format."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new Response<string>
                {
                    Success = false,
                    Code = 500,
                    Data = (ex.Message + ex.InnerException)
                });
            }
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.categoryService.DeleteCategory(id);
                    _unitOfWork.Save();
                    return Ok(new Response<bool>
                    {
                        Success = result,
                        Data = result,
                        Code = 200
                    });
                }
                else
                {
                    return Ok(new Response<string>
                    {
                        Success = false,
                        Code = 400,
                        Data = "Request body is not in correct format."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new Response<string>
                {
                    Success = false,
                    Code = 500,
                    Data = (ex.Message + ex.InnerException)
                });
            }
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllCategories(string sortOrder = "")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.categoryService.GetAllCategories(sortOrder);
                    return Ok(new Response<List<Category>>
                    {
                        Success = result != null,
                        Data = result,
                        Code = 200
                    });
                }
                else
                {
                    return Ok(new Response<string>
                    {
                        Success = false,
                        Code = 400,
                        Data = "Request body is not in correct format."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new Response<string>
                {
                    Success = false,
                    Code = 500,
                    Data = (ex.Message + ex.InnerException)
                });
            }
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.categoryService.GetCategoryById(id);
                    return Ok(new Response<Category>
                    {
                        Success = result != null,
                        Data = result,
                        Code = 200
                    });
                }
                else
                {
                    return Ok(new Response<string>
                    {
                        Success = false,
                        Code = 400,
                        Data = "Request body is not in correct format."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new Response<string>
                {
                    Success = false,
                    Code = 500,
                    Data = (ex.Message + ex.InnerException)
                });
            }
        }
    }
}
