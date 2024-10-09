using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Store.Web.ExtensionMethod
{
    public static class SwagerDocumentationExtension
    {
        public static IServiceCollection AddSwagerDocumntation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "StoreApi",
                    Contact = new OpenApiContact
                    {
                        Name = "Yassin Mohamed",
                        Email = "ym9807770@gmail.com"
                    }
                });


                var securityschema = new OpenApiSecurityScheme
                {
                    Description = "Jwt Descripe Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Id = "bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition("bearer", securityschema);

                var securityreq = new OpenApiSecurityRequirement
                {
                    {securityschema,new[] { "bearer" } }
                };

                options.AddSecurityRequirement(securityreq);
            });

            return services;
        } 
    }
}
