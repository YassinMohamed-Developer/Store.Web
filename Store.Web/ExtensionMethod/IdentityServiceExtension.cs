using Castle.Core.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using Store.Data.Context;
using Store.Data.Entity.IdentityEntity;
using System.Text;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;


namespace Store.Web.ExtensionMethod
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection ApplyIdentityService(this IServiceCollection services,IConfiguration config)
        {
            var builder = services.AddIdentityCore<AppUser>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);

            builder.AddEntityFrameworkStores<StoreIdentityDbContext>();

            builder.AddSignInManager<SignInManager<AppUser>>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        ValidateIssuer = true,
                        ValidIssuer = config["Token:Issuer"],
                        ValidateAudience = false,
                    };
                });

            return services;
        }
    }
}
