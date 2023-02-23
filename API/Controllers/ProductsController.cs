using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        // these are dependency injection
        private readonly IGenericRepository<Product> _productContext;
        private readonly IGenericRepository<ProductBrand> _productBrandContext;
        private readonly IGenericRepository<ProductType> _productTypeContext;
        private readonly IMapper _mapper;

        // private readonly IProductRepository _productContext;

        public ProductsController(
            IGenericRepository<Product> productContext, 
            IGenericRepository<ProductBrand> productBrandContext,
            IGenericRepository<ProductType> productTypeContext,
            IMapper mapper)
        {
            _productContext = productContext;
            _productBrandContext = productBrandContext;
            _productTypeContext = productTypeContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            // this code reaches out to database
            var products = await _productContext.ListAsync(spec);
            // At this point, these codes are not hitting our database
            // its running from memory
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productContext.GetEntityWithSpec(spec);

            if (product == null)
                return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductDto>(product);
        }

        [HttpGet("ProductBrands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandContext.ListAllAsync());
        }

        [HttpGet("ProductBrands/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductBrand>> GetProductBrand(int id)
        {
            var brand = await _productBrandContext.GetByIdAsync(id);

            if (brand == null)
                return NotFound(new ApiResponse(404));
            
            return brand;
        }
 
        [HttpGet("ProductTypes")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeContext.ListAllAsync());
        }

        [HttpGet("ProductTypes/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // this helps us return data in different format
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] // in this case, return in our ApiResponse format
        public async Task<ActionResult<ProductType>> GetProductType(int id)
        {
            var type = await _productTypeContext.GetByIdAsync(id);

            if (type == null)
                return NotFound(new ApiResponse(404));
            
            return type;
        }
    }
}