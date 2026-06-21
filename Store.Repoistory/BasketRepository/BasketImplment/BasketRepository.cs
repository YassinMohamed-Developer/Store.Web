using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Store.Repoistory.BasketRepository.BasketInterface;
using Store.Repoistory.BasketRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repoistory.BasketRepository.BasketImplment
{
    public class BasketRepository : IBasketRepository
    {
		private readonly IDistributedCache _distributedCache;

		public BasketRepository(IDistributedCache distributedCache)
        {
			_distributedCache = distributedCache;
		}

		public async Task<string> AddToBasket(CustomerBasket basket)
		{
            var existingBasket = await _distributedCache.GetStringAsync(basket.Id.ToString());

            if(existingBasket != null)
            {
                var existingBasketObj = JsonSerializer.Deserialize<CustomerBasket>(existingBasket);

                foreach(var newitem in basket.BasketItems)
                {
                    var existingItem = existingBasketObj.BasketItems.FirstOrDefault(x => x.ProductId == newitem.ProductId);
                    if(existingItem != null)
                    {
                        existingItem.Quantity += newitem.Quantity;
                    }
                    else
                    {
                        existingBasketObj.BasketItems.Add(newitem);
                    }
				}
                basket = existingBasketObj;
			}

			await _distributedCache.SetStringAsync(basket.Id.ToString(),
                System.Text.Json.JsonSerializer.Serialize(basket), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                });
            return basket.Id.ToString();
		}

		public async Task<bool> DeleteBasketAsync(string Id)
        {
            await _distributedCache.RemoveAsync(Id);
            return true;
		}

        public async Task<CustomerBasket> GetBasketAsync(string Id)
        {
            var basketjson = await _distributedCache.GetStringAsync(Id);

            var basket = JsonSerializer.Deserialize<CustomerBasket>(basketjson);

            if(basket == null)
            {
                return null;
            }

            return basket;
		}
        public async Task<string> UpdateBasketAsync(CustomerBasket basket)
        {
             await _distributedCache.SetStringAsync(basket.Id.ToString(),
                System.Text.Json.JsonSerializer.Serialize(basket), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
			    });

            return basket.Id.ToString();
        }
    }
}
