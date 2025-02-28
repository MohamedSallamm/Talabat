using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithFiltrationForCountSpecifications : BaseSpecifications<Product>
    {

        public ProductWithFiltrationForCountSpecifications(ProductSpecParams specParams) 
            : base(P =>
                  (string.IsNullOrEmpty(specParams.Search) || P.Name.Contains(specParams.Search)) &&
                  (!specParams.brandId.HasValue || P.BrandId == specParams.brandId.Value) &&
                  (!specParams.categoryId.HasValue || P.CategoryId == specParams.categoryId.Value)
            )
        {
            
        }
    }
}
