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
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(ILogger<ProductController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> InsertProduct(Product entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.productService.InsertProduct(entity);
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
        public async Task<IActionResult> UpdateProduct(Product entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.productService.UpdateProduct(entity);
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
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.productService.DeleteProduct(id);
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
        public async Task<IActionResult> GetAllProducts(string sortOrder = "")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.productService.GetAllProducts(sortOrder);
                    return Ok(new Response<List<Product>>
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
        [Route("getbycategory/{category}")]
        public async Task<IActionResult> GetProductByCategory(string category, string sortOrder = "")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.productService.GetProductsByCategory(category, sortOrder);
                    return Ok(new Response<List<Product>>
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
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.productService.GetProductById(id);
                    return Ok(new Response<Product>
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
