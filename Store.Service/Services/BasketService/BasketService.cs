using AutoMapper;
using Store.Repoistory.BasketRepository.BasketInterface;
using Store.Repoistory.BasketRepository.Models;
using Store.Service.Services.BasketService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

		public async Task<string> AddToBasketAsync(CustomerBasketRequestDto basket)
		{

            var customerBasket = new CustomerBasket 
            { 
                DeliveryMethodId = basket.DeliveryMethodId,
                ShippingPrice = basket.ShippingPrice,
                PaymentIntentId = basket.PaymentIntentId,
                ClientSecret = basket.ClientSecret,
                BasketItems = basket.BasketItems.Select(item => new BasketItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    PictureUrl = item.PictureUrl,
                    BrandName = item.BrandName,
                    TypeName = item.TypeName,
				}).ToList(),
                Id = GenerateNewId()
			};

            await _basketRepository.AddToBasket(customerBasket);

            return customerBasket.Id.ToString();
		}

		public async Task<bool> DeleteBasketAsync(string Id)
            => await _basketRepository.DeleteBasketAsync(Id);

        public async Task<CustomerBasketDto> GetBasketAsync(string Id)
        {
            var basket = await _basketRepository.GetBasketAsync(Id);

            if (basket == null)
            {
                return new CustomerBasketDto();
            }

            var MapBasket = _mapper.Map<CustomerBasketDto>(basket);

            return MapBasket;
        }

        public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket)
        {
            var customerbasket = _mapper.Map<CustomerBasket>(basket);

            var updateBasket = await _basketRepository.UpdateBasketAsync(customerbasket);

            var getBasket = await _basketRepository.GetBasketAsync(updateBasket);

            var MapBasket = _mapper.Map<CustomerBasketDto>(getBasket);

            return MapBasket;
        }
        private string GenerateNewId()
        {
            Random random = new Random();

            int randomId = random.Next(1000,10000);

            return $"BS-{randomId}";
        }
    }
}
