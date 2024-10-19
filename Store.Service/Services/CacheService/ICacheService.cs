using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.CacheService
{
    public interface ICacheService
    {
        Task<string> GetCacheResponseAsync(string Key);

        Task SetCacheResponseAsync(string Key, object Response, TimeSpan TimeToLive);
    }
}
