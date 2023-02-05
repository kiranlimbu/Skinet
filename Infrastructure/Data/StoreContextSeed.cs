
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            // all this work is being done in memory
            if (!context.ProductBrands.Any())
            {
                // get data/file (at this stage data=unreadable)
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                // deserialize data/file (make it readable)
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                // add (readable)data to database
                context.ProductBrands.AddRange(brands);
            }

            if (!context.ProductTypes.Any())
            {
                // get data/file (at this stage data=unreadable)
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                // deserialize data/file (make it readable)
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                // add (readable)data to database
                context.ProductTypes.AddRange(types);
            }

            if (!context.Products.Any())
            {
                // get data/file (at this stage data=unreadable)
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                // deserialize data/file (make it readable)
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                // add (readable)data to database
                context.Products.AddRange(products);
            }

            // track changes to database 
            // (changes made in memory will be updated into database, hence "await")
            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
        }
    }
}