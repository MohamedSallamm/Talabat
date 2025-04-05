using AutoMapper;
using Microsoft.AspNetCore.Authorization;

//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Helpers;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Product_Specs;


namespace Talabat.API.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;


        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;

        }


        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Cached(600)] // Action filter to cache the response of this action
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams Params)
        {
            // 1️- Create Specification (Correct)
            var Spec = new ProductWithBrandAndCategorySpecifications(Params);

            // 2️- Fetch Products using the Specification from Repository (FIXED)
            var products = await _productService.GetProductsAsync(Spec);

            // 3️- Get Count for Pagination
            var Count = await _productService.GetCountAsync(Spec);

            // 4️- Map Products to DTO (FIXED)   
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            // 5️- Return Paginated Result
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, data, Count));
        }

        //Get Product By Id

        [Authorize]
        [Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {

            var Product = await _productService.GetProductByIdAsync(id);
            if (Product is null)
                return NotFound(new APIResponse(404));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(Product));
        }

        [Cached(600)]
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);
        }

        //BaseUrl/api/Products/type
        [Cached(600)]
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetTypes()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }



    }
}
