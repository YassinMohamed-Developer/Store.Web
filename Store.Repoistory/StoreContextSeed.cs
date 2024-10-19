using Microsoft.Extensions.Logging;
using Store.Data.Context;
using Store.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repoistory
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context,ILoggerFactory loggerFactory)
        {
			try
			{
				if(context.ProductBrands is not null && !context.ProductBrands.Any())
				{
					//Presist data to database

					var brandsData = File.ReadAllText("../Store.Repoistory/SeedData/brands.json");

                    //Serlization/Deserlization
                    //Serlization => Convert Object convert to Text(string)
                    //Deserlization => Convert Text(string) to Object(Table)

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    if(brands is not null)
                    {
                        //To Add In Database
                        await context.ProductBrands.AddRangeAsync(brands);
                    }
                }
                if(context.ProductTypes is not null && !context.ProductTypes.Any())
                {
                    var TypeData = File.ReadAllText("../Store.Repoistory/SeedData/types.json");

                    var types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);

                    if(types is not null)
                    {
                        await context.ProductTypes.AddRangeAsync(types);
                    }
                }
                if(context.DeliveryMethods is not null && !context.DeliveryMethods.Any())
                {
                    var deliverymethod = File.ReadAllText("../Store.Repoistory/SeedData/delivery.json");

                    var delivery = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverymethod);

                    if(delivery is not null)
                    {
                        await context.DeliveryMethods.AddRangeAsync(delivery);
                    }
                }

                if (context.Products is not null && !context.Products.Any())
                {
                    var ProductData = File.ReadAllText("../Store.Repoistory/SeedData/products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                    if (products is not null)
                    {
                        await context.Products.AddRangeAsync(products);
                    }
                }

                await context.SaveChangesAsync();
            }

			catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
			}
        }
    }
}
