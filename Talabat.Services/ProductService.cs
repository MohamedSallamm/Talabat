using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.Services
{
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork<Product> _unitOfWork;

        public ProductService(IUnitOfWork<Product> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductWithBrandAndCategorySpecifications Params)
        {
            var Spec = new ProductWithBrandAndCategorySpecifications(Params);
            var products = await _unitOfWork.repository<Product>().GetAllWithSpecAsync(Spec);
            return (IReadOnlyList<Product>)products;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var Spec = new ProductWithBrandAndCategorySpecifications(id);
            var Product = await _unitOfWork.repository<Product>().GetEntityWithSpecAsync(Spec);
            return Product;
        }

        public async Task<int> GetCountAsync(ProductWithBrandAndCategorySpecifications Params)
        {
            var CountSpec = new ProductWithFiltrationForCountSpecifications(Params);
            var Count = await _unitOfWork.repository<Product>().GetCountAsync(CountSpec);
            return Count;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
          => (IReadOnlyList<ProductBrand>)await _unitOfWork.repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
          => (IReadOnlyList<ProductCategory>)await _unitOfWork.repository<ProductCategory>().GetAllAsync();

        //public Task<IReadOnlyList<Product>> GetProductsAsync(ProductWithBrandAndCategorySpecifications spec)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
