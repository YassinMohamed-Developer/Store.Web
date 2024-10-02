using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.Services.CacheService;
using System.Text;

namespace Store.Web.Helper
{
    public class CacheAttribute : Attribute,IAsyncActionFilter
    {
        private readonly int _timeToLive;

        public CacheAttribute(int timeToLive)
        {
            _timeToLive = timeToLive;
        }

        //Go To Cache First to check that respnse is in the cache or not?
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheservice = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cachekey = GenerateKeyFromRequest(context.HttpContext.Request);

            var cacheresponse = await cacheservice.GetCacheResponseAsync(cachekey);

            if (!string.IsNullOrEmpty(cacheresponse))
            {
                var cachecontent = new ContentResult
                {
                    Content = cacheresponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };

                context.Result = cachecontent;

                return;
            }

            var excutedrequest = await next();

            if (excutedrequest.Result is OkObjectResult response)
            {
                cacheservice.SetCacheResponseAsync(cachekey, response.Value, TimeSpan.FromSeconds(_timeToLive));
            }

        }

        private string GenerateKeyFromRequest(HttpRequest request)
        {
            StringBuilder cachekey = new StringBuilder();

            cachekey.Append($"{request.Path}");

            foreach (var (key,value) in request.Headers.OrderBy(x => x.Key))
            {
                cachekey.Append($"|{key}|{value}");
            }

            return cachekey.ToString();
        }
    }
}
