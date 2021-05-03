using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingModule.API.Interfaces;
using System;
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
    }
}
