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
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _environment.IsDevelopment() ?
                    new CustomException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new CustomException((int)HttpStatusCode.InternalServerError);

                var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower};

                //Object to Json File(string) you need to make Serialization

                var json = JsonSerializer.Serialize(response,option);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
