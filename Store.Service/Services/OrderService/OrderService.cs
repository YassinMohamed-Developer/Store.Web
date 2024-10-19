using AutoMapper;
using Store.Data.Entity;
using Store.Data.Entity.OrderEntity;
using Store.Repoistory.BasketRepository.BasketInterface;
using Store.Repoistory.Interfaces;
using Store.Repoistory.Specification;
using Store.Service.Services.BasketService;
using Store.Service.Services.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketService basketService,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrderDetailsDto> CreateOrderAsync(OrderDto input)
        {
            #region Get The Basket
            var basket = await _basketService.GetBasketAsync(input.BasketId);

            if (basket is null)
                throw new Exception("No Order Created Because the basket is Empty");
            #endregion

            #region Fill The List of OrderItem 

            var OrderItems = new List<OrderItemDto>();

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);

                if (productItem is null)
                    throw new Exception($"Product with Id {item.ProductId} Not Exist");

                var itemorder = new ProductItem
                {
                    ProductId = productItem.Id,
                    PictureUrl = productItem.PictureUrl,
                    ProductName = productItem.Name,
                };

                var orderitem = new OrderItem
                {
                    Price = productItem.Price,
                    Quantity = item.Quantity,
                    ProductItem = itemorder,
                };

                var mappedorder = _mapper.Map<OrderItemDto>(orderitem);

                OrderItems.Add(mappedorder);

            } 
            #endregion

            #region Calculate The SubTotal
            var subtotal = OrderItems.Sum(order => order.Quantity * order.Price);
            #endregion

            #region Get Delivery Method

            var deliverymethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);

            if (deliverymethod is null)
                throw new Exception("Delivery Method not exist");
            #endregion

            #region Payment => To Do

            #endregion

            #region Create Order
            var mapShippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);

            var mapOrderItems = _mapper.Map<List<OrderItem>>(OrderItems);

            var order = new Order
            {
                OrderItems = mapOrderItems,
                ShippingAddress = mapShippingAddress,
                BasketId = input.BasketId,
                SubTotal = subtotal,
                DeliveryMethodId = deliverymethod.Id,
                BuyerEmail = input.BuyerEmail,
            };

            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
            await _unitOfWork.CompleteAsync(); 
            #endregion 

            var mappedOrderDetailsDto = _mapper.Map<OrderDetailsDto>(order);

            return mappedOrderDetailsDto;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync()
            => await _unitOfWork.Repository<DeliveryMethod,int>().GetAllAsync();

        public async Task<OrderDetailsDto> GetOrderByIdAsync(Guid OrderId)
        {
           var specs = new OrderSpecs(OrderId);

            var order = await _unitOfWork.Repository<Order,Guid>().GetByIdWithSpecificationAsync(specs);

            if (order is null)
                throw new Exception("No Order with this Id");

            var mapeOrder = _mapper.Map<OrderDetailsDto>(order);

            return mapeOrder;
        }

        public async Task<IReadOnlyList<OrderDetailsDto>> GetOrdersForUserAsync(string BuyerEmail)
        {
            var specs = new OrderSpecs(BuyerEmail);

            var order = await _unitOfWork.Repository<Order,Guid>().GetAllWithSpecificationAsync(specs);

            if (order is { Count: <= 0 })
                throw new Exception("There No Order Yet");

            var mapeOrder = _mapper.Map<IReadOnlyList<OrderDetailsDto>>(order);

            return mapeOrder;
        }
    }
}
