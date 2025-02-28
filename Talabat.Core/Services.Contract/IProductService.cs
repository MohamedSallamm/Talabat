using Talabat.Core.Entities;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.Core.Services.Contract
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams Params);
        Task<Product?> GetProductByIdAsync(int id);
        Task<int> GetCountAsync(ProductSpecParams Params);

        Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        //Task<IReadOnlyList<Product>> GetProductsAsync(ProductWithBrandAndCategorySpecifications spec);
    }
}
