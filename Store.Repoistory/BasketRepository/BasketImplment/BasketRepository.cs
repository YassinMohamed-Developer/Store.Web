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
        private readonly IDatabase  _database;

        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string Id)
            => await _database.KeyDeleteAsync(Id);

        public async Task<CustomerBasket> GetBasketAsync(string Id)
        {
            //string =>Object ==== Deserlization
            var custometbasket =  await _database.StringGetAsync(Id);

            return custometbasket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(custometbasket);
        }
        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var IsCreated = await _database.StringSetAsync(basket.Id,JsonSerializer.Serialize(basket),TimeSpan.FromDays(30));

            if (!IsCreated)
                return null;

            return await GetBasketAsync(basket.Id);
        }
    }
}
