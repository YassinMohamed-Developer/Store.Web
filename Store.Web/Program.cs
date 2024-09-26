
using Microsoft.EntityFrameworkCore;
using Store.Data.Context;
using Store.Repoistory;
using Store.Repoistory.Interfaces;
using Store.Repoistory.Repositories;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dto;
using Store.Web.Helper;
namespace Store.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(ProductProfile));
            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            //NoT Recomended to write bulk of code in the program
            #region Seeding
            //using (var scope = app.Services.CreateScope())
            //{
            //    // to give me object of context and logfactory
            //    var services = scope.ServiceProvider;

            //    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            //    try
            //    {
            //        var context = services.GetRequiredService<StoreDbContext>();

            //        await StoreContextSeed.SeedAsync(context, loggerFactory);
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = loggerFactory.CreateLogger<Program>();
            //        logger.LogError(ex.Message);
            //    }
            //} 
            #endregion 

            await ApplySeeding.ApplySeedingAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
