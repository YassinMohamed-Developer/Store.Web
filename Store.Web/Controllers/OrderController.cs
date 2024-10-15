using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Store.Data.Entity;
using Store.Service.HandleException;
using Store.Service.Services.OrderService;
using Store.Service.Services.OrderService.Dto;
using System.Security.Claims;

namespace Store.Web.Controllers
{
    [Authorize]
    public class OrderController : BasicsController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetailsDto>> CreateOrderAsync(OrderDto orderDto)
        {
            var order = await _orderService.CreateOrderAsync(orderDto);

            if(order is null)
            {
                return BadRequest(new Response(400, "Error While Creating The Order"));
            }

            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDetailsDto>>> GetOrdersForUserAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if(email is null)
                return BadRequest(new Response(400, "This Email Don't Have Orders"));

            var ordersByEmail = await _orderService.GetOrdersForUserAsync(email);


            return Ok(ordersByEmail);
        }
        [HttpGet]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderByIdAsync(Guid Id)
            =>Ok(await _orderService.GetOrderByIdAsync(Id));

        [HttpGet]
        public async Task<ActionResult<DeliveryMethod>> GetAllDeliveryMethodAsync()
            =>Ok(await _orderService.GetAllDeliveryMethodAsync());
    }
}
