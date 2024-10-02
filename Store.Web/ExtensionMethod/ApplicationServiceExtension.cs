using Store.Repoistory.Interfaces;
using Store.Repoistory.Repositories;
using Store.Service.Services.CacheService;
using Store.Service.Services.ProductService.Dto;
using Store.Service.Services.ProductService;
using Microsoft.AspNetCore.Mvc;
using Store.Service.HandleException;

namespace Store.Web.ExtensionMethod
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection ApplyService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICacheService, CacheService>();

            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = actioncontext =>
                {
                    var errors = actioncontext.ModelState
                                    .Where(model => model.Value?.Errors.Count() > 0)
                                    .SelectMany(error => error.Value.Errors)
                                    .Select(x => x.ErrorMessage)
                                    .ToList();

                    var errorResponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}
