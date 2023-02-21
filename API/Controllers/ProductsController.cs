using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // these are dependency injection
        private readonly ILogger<ProductsController> _logger;
        private readonly IGenericRepository<Product> _productContext;
        private readonly IGenericRepository<ProductBrand> _productBrandContext;
        private readonly IGenericRepository<ProductType> _productTypeContext;
        private readonly IMapper _mapper;

        // private readonly IProductRepository _productContext;

        public ProductsController(
            ILogger<ProductsController> logger, 
            IGenericRepository<Product> productContext, 
            IGenericRepository<ProductBrand> productBrandContext,
            IGenericRepository<ProductType> productTypeContext,
            IMapper mapper)
        {
            _logger = logger;
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
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productContext.GetEntityWithSpec(spec);

            return _mapper.Map<Product, ProductDto>(product);
        }

        [HttpGet("ProductBrands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandContext.ListAllAsync());
        }

        [HttpGet("ProductBrands/{id}")]
        public async Task<ActionResult<ProductBrand>> GetProductBrand(int id)
        {
            return await _productBrandContext.GetByIdAsync(id);
        }

        [HttpGet("ProductTypes")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeContext.ListAllAsync());
        }

        [HttpGet("ProductTypes/{id}")]
        public async Task<ActionResult<ProductType>> GetProductType(int id)
        {
            return await _productTypeContext.GetByIdAsync(id);
        }
    }
}