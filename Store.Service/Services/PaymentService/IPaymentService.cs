using Store.Repoistory.BasketRepository.Models;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentAsync(CustomerBasketDto customerBasket);

        Task<OrderDetailsDto> UpdateOrderPaymentSucceeded(string paymentIntentId);

        Task<OrderDetailsDto> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
