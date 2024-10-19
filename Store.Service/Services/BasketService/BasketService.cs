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
            if(basket.Id is null)
            {
                basket.Id = GenerateNewId();
            }

            var customerbasket = _mapper.Map<CustomerBasket>(basket);

            var updateBasket = await _basketRepository.UpdateBasketAsync(customerbasket);

            var MapBasket = _mapper.Map<CustomerBasketDto>(updateBasket);

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
