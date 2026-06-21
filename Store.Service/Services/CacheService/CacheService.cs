using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Store.Data.Context;
using Store.Data.Entity;
using Store.Repoistory.Interfaces;
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
        private readonly IDistributedCache _distributedCache;
		private readonly StoreDbContext _storeDbContext;

		public CacheService(IDistributedCache distributedCache,StoreDbContext storeDbContext)
        {
            //_database it is make get the response cache and set the response cache
            _distributedCache = distributedCache;
			_storeDbContext = storeDbContext;
		}
        public async Task<string> GetCacheResponseAsync(string Key)
        {

			var cachedResponse = await _distributedCache.GetStringAsync(Key);
            if (string.IsNullOrEmpty(cachedResponse))
            {
                return null;
            }

			return cachedResponse.ToString();
        }

        public async Task SetCacheResponseAsync(string Key, object Response, TimeSpan TimeToLive)
        {
            if (Response is null)
                return;

   //         var exisitingResponse = await _distributedCache.GetStringAsync(Key);

   //         if (!string.IsNullOrEmpty(exisitingResponse))
   //         {
   //             var existingResponseObj = JsonSerializer.Deserialize<object>(exisitingResponse);

   //             foreach(var newItem in Response.ToString())
   //             {
   //                 var existingItem = existingResponseObj.ToString().FirstOrDefault(x => x == newItem);

   //                 if(existingItem != null)
   //                 {
   //                     continue;
   //                 }
   //                 else
   //                 {
   //                     existingResponseObj.ToString().Append(newItem);
			//		}
			//	}
   //             Response = existingResponseObj;
			//}

			var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            await _distributedCache.SetStringAsync(Key, System.Text.Json.JsonSerializer.Serialize(Response, option),
               new DistributedCacheEntryOptions
               {
                   AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(TimeToLive.Hours),
               });
        }
    }
    
}
