using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entites;
using Talabat.Core.Reporitories.Contract;
using Talabat.Core.Sepecifications;
using Talabat.Core.Sepecifications.ProductSpecifications;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<ProductBrand> _brandRepo;
        ///private readonly IGenericRepository<ProductCategory> _categoryRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            ///IGenericRepository<Product> productRepo,
            ///IGenericRepository<ProductBrand> brandRepo,
            ///IGenericRepository<ProductCategory> categoryRepo,
            IProductService productService,
            IMapper mapper)
        {
            ///_productRepo = productRepo;
            ///_brandRepo = brandRepo;
            ///_categoryRepo = categoryRepo;
            _mapper = mapper;
            _productService = productService;
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<ProductToReturnDto>>> GetProductsAsync([FromQuery] ProductSpecsParams specParams)
        {
            var products = await _productService.GetProductsAsync(specParams);

            var count = await _productService.GetCountAsync(specParams);

            var res = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new PaginationResponse<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, res, count));
        }

        //[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product is null)
                return NotFound(new ApiResponse(400));

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductsBrandsAsync()
        {
            var brands = await _productService.GetBrandsAsync();

            return Ok(brands);
        }

        [Cached(600)]
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetProductsCategoriesAsync()
        {
            var categories = await _productService.GetCategoriesAsync();

            return Ok(categories);
        }

    }

}
