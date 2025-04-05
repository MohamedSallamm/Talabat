using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            : base(P =>
                  (string.IsNullOrEmpty(specParams.Search) || P.Name.Contains(specParams.Search)) &&
                  (!specParams.brandId.HasValue || P.BrandId == specParams.brandId.Value) &&
                  (!specParams.categoryId.HasValue || P.CategoryId == specParams.categoryId.Value)
            )

        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductCategory);

            if (!string.IsNullOrEmpty(specParams.sort))
            {
                switch (specParams.sort)
                {
                    case "priceAsc":
                        AddOrderByAsc(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderByAsc(P => P.Name);
                        break;
                }
            }
            else
                AddOrderByAsc(P => P.Name);

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);


        }

        public ProductWithBrandAndCategorySpecifications(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductCategory);

        }

        public ProductWithBrandAndCategorySpecifications(ProductWithBrandAndCategorySpecifications @params)
        {
            Params = @params;
        }

        public ProductWithBrandAndCategorySpecifications Params { get; }
    }
}
