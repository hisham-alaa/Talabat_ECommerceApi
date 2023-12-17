using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Repository.Data.Contexts
{
    public static class StoreContextSeed
    {
        public static async Task SeedDataAsync(StoreContext context)
        {
            if (context.Brands.Count() == 0)
            {
                var brandsText = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");

                var brandsData = JsonSerializer.Deserialize<List<ProductBrand>>(brandsText);

                if (brandsData?.Count > 0)
                    foreach (var brand in brandsData)
                        context.Brands.Add(brand);
                await context.SaveChangesAsync();
            }
            if (context.Categories.Count() == 0)
            {
                var categoriesText = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json");

                var categoriesData = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesText);

                if (categoriesData?.Count > 0)
                    foreach (var category in categoriesData)
                        context.Categories.Add(category);
                await context.SaveChangesAsync();
            }
            if (context.Products.Count() == 0)
            {
                var productsText = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");

                var productsData = JsonSerializer.Deserialize<List<Product>>(productsText);

                if (productsData?.Count > 0)
                    foreach (var product in productsData)
                        context.Products.Add(product);
                await context.SaveChangesAsync();
            }
            if (context.DeliveryMethods.Count() == 0)
            {
                var DeliveryMethodsText = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");

                var DeliveryMethodsData = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsText);

                if (DeliveryMethodsData?.Count > 0)
                    foreach (var deliveryMethod in DeliveryMethodsData)
                        context.DeliveryMethods.Add(deliveryMethod);
                await context.SaveChangesAsync();
            }

        }
    }
}
