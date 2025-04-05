using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedASync(StoreContext _dbContext)
        {
            if (_dbContext.ProductBrands.Count() == 0)
            {
                var BrandData = File.ReadAllText("..//Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);

                if (Brands?.Count() > 0)
                    foreach (var brand in Brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(brand);
                    }
                await _dbContext.SaveChangesAsync();
            }

            if (_dbContext.ProductCategories.Count() == 0)
            {
                var CategoriesData = File.ReadAllText("..//Talabat.Repository/Data/DataSeed/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);

                if (Categories?.Count() > 0)
                    foreach (var Category in Categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(Category);
                    }
                await _dbContext.SaveChangesAsync();
            }

            if (_dbContext.Products.Count() == 0)
            {
                var ProductsData = File.ReadAllText("..//Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (Products?.Count() > 0)
                {
                    foreach (var Product in Products)
                    {
                        _dbContext.Set<Product>().Add(Product);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (_dbContext.DeliveryMethods.Count() == 0)
            {
                var deliveryMethodsData = File.ReadAllText("..//Talabat.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethodsData?.Count() > 0)
                {
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

        }
    }
}
