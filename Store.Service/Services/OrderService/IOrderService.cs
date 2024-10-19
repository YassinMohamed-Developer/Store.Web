using Store.Data.Entity;
using Store.Service.Services.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService
{
    public interface IOrderService
    {
        Task<OrderDetailsDto> CreateOrderAsync(OrderDto input);

        Task<IReadOnlyList<OrderDetailsDto>> GetOrdersForUserAsync(string BuyerEmail);

        Task<OrderDetailsDto> GetOrderByIdAsync(Guid OrderId);
        Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync();
    }
}
