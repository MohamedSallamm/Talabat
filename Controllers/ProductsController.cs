using AutoMapper;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
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


        // Get All Products
        [HttpGet]
        //[Authorize/*(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)*/]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndCategorySpecifications(Params);
            var Count = await _productService.GetCountAsync(Params);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>((IReadOnlyList<Product>)Spec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, data, Count));
        }


        //[HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams Params)
        //{
        //    // Create the specification for filtering/sorting
        //    var spec = new ProductSpecParams(Params);

        //    // Use the specification to query the database and get the products
        //    var products = await _productService.GetProductsAsync(spec); // Ensure this method exists and uses the specification

        //    // Get the total count of products matching the criteria
        //    var count = await _productService.GetCountAsync(Params);

        //    // Map the products to the DTO
        //    var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

        //    // Return the paginated response
        //    return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, data, count));
        //}


        //Get Product By Id

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetPRoduct(int id)
        {

            var Product = await _productService.GetProductByIdAsync(id);
            if (Product is null)
                return NotFound(new APIResponse(404));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(Product));
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);
        }

        //BaeUrl/api/Products/type
        [HttpGet("Category")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetTypes()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }



    }
}
