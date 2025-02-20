using Store.Service.HandleException;
using System.Net;
using System.Text.Json;

namespace Store.Web.customMiddleware
{
    public class MiddlewareException
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<MiddlewareException> _logger;

        public MiddlewareException(RequestDelegate next,IHostEnvironment environment,ILogger<MiddlewareException> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Response ex)
            {
                var response = context.Response;
                context.Response.ContentType = "application/json";

                var Devresponse = new BaseResult<string> { IsSucess = false, Errors = [ex.Message] };

                switch (ex)
                {
                    case Response e:
                        response.StatusCode = e.StatusCode;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;

                }

                var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper};

                //Object to Json File(string) you need to make Serialization

                var json = JsonSerializer.Serialize(Devresponse,option);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
