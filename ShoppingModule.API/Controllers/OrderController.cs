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
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(ILogger<OrderController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("purchase")]
        public async Task<IActionResult> PurchaseOrder(Order entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.orderService.PurchaseOrder(entity);
                    _unitOfWork.Save();
                    return Ok(new Response<string>
                    {
                        Success = !string.IsNullOrEmpty(result),
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
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.orderService.GetAllOrders();
                    return Ok(new Response<List<Order>>
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
