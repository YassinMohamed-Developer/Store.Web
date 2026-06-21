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
              await cacheservice.SetCacheResponseAsync(cachekey, response.Value, TimeSpan.FromHours(_timeToLive));
            }

        }

        private string GenerateKeyFromRequest(HttpRequest request)
        {
            StringBuilder cachekey = new StringBuilder();


            foreach (var key in request.Query)
            {
			    cachekey.Append($"{request.Path}|{key.Key}:{key.Value}|");
            }

            return cachekey.ToString();
        }
    }
}
