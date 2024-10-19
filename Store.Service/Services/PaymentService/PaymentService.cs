using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entity;
using Store.Data.Entity.OrderEntity;
using Store.Repoistory.Interfaces;
using Store.Repoistory.Specification;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.OrderService.Dto;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Data.Entity.Product;

namespace Store.Service.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;

        public PaymentService(IConfiguration configuration,IUnitOfWork unitOfWork,IBasketService basketService
            ,IMapper mapper)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _mapper = mapper;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentAsync(CustomerBasketDto customerBasket)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

            if (customerBasket is null)
                throw new Exception("Basket Is Empty");

            var deliverymethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(customerBasket.DeliveryMethodId);

            if(deliverymethod is null)
                throw new Exception("Delivery Method is not provided");

            decimal shippingPrice = deliverymethod.Price;

            foreach (var item in customerBasket.BasketItems)
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);
 
                if (product is null)
                    throw new Exception($"No Product with this Id : {item.ProductId}");

                if (item.Price != product.Price)
                {
                    item.Price = product.Price;
                }
                if(item.PictureUrl != product.PictureUrl)
                    item.PictureUrl = product.PictureUrl;
                if(item.ProductName != product.Name)
                    item.ProductName = product.Name;

            }

            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(customerBasket.PaymentIntentId))
            {
                var oprions = new PaymentIntentCreateOptions
                {
                    Amount = (long)customerBasket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)(shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                paymentIntent = await service.CreateAsync(oprions);
                customerBasket.PaymentIntentId = paymentIntent.Id;
                customerBasket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)customerBasket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)(shippingPrice * 100),
                };
                paymentIntent = await service.UpdateAsync(customerBasket.PaymentIntentId,options);
            }

            await _basketService.UpdateBasketAsync(customerBasket);

            return customerBasket;
        }

        public async Task<OrderDetailsDto> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var specs = new PaymentSpecs(paymentIntentId);

            var order = await _unitOfWork.Repository<Order,Guid>().GetByIdWithSpecificationAsync(specs);

            if (order is null)
                throw new Exception("Order Doesn't Exist");

            order.OrderPaymentStatus = OrderPaymentStatus.Failed;

            _unitOfWork.Repository<Order, Guid>().Update(order);

            await _unitOfWork.CompleteAsync();

            var mappOrder = _mapper.Map<OrderDetailsDto>(order);

            return mappOrder;
        }

        public async Task<OrderDetailsDto> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var specs = new PaymentSpecs(paymentIntentId);

            var order = await _unitOfWork.Repository<Order, Guid>().GetByIdWithSpecificationAsync(specs);

            if (order is null)
                throw new Exception("Order Doesn't Exist");

            order.OrderPaymentStatus = OrderPaymentStatus.Received;

            _unitOfWork.Repository<Order, Guid>().Update(order);

            await _unitOfWork.CompleteAsync();

            await _basketService.DeleteBasketAsync(order.BasketId);

            var mappOrder = _mapper.Map<OrderDetailsDto>(order);

            return mappOrder;
        }
    }
}
