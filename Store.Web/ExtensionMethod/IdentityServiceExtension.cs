using Microsoft.AspNetCore.Identity;
using Microsoft.SqlServer.Server;
using Store.Data.Context;
using Store.Data.Entity.IdentityEntity;

namespace Store.Web.ExtensionMethod
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection ApplyIdentityService(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<AppUser>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);

            builder.AddEntityFrameworkStores<StoreIdentityDbContext>();

            builder.AddSignInManager<SignInManager<AppUser>>();


            services.AddAuthentication();

            return services;
        }
    }
}
