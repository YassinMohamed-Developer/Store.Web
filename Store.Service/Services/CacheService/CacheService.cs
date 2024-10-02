using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Service.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            //_database it is make get the response cache and set the response cache
            _database = redis.GetDatabase();
        }
        public async Task<string> GetCacheResponseAsync(string Key)
        {
           var cachedResponse = await _database.StringGetAsync(Key);

            if (cachedResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cachedResponse.ToString();
        }

        public async Task SetCacheResponseAsync(string Key, object Response, TimeSpan TimeToLive)
        {
            if (Response is null)
                return;

            ///var cahshedResponse = await _database.StringSetAsync(Key,Response,TimeToLive);


            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var Json = JsonSerializer.Serialize(Response, option);

            await _database.StringSetAsync(Key,Json,TimeToLive);
        }
    }
}
