using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithFiltrationForCountSpecifications : BaseSpecifications<Product>
    {

        public ProductWithFiltrationForCountSpecifications(ProductSpecParams productParams)
      : base(p =>
          (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)) &&
          (!productParams.brandId.HasValue || p.BrandId == productParams.brandId) &&
          (!productParams.categoryId.HasValue || p.CategoryId == productParams.categoryId))
        {
            Console.WriteLine($"Applying Pagination in Specification: PageIndex = {productParams.PageIndex}, PageSize = {productParams.PageSize}");
            ApplyPagination((productParams.PageIndex - 1) * productParams.PageSize, productParams.PageSize);
        }

        public ProductWithFiltrationForCountSpecifications(ProductWithBrandAndCategorySpecifications @params)
        {
        }
    }
}
