using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productContext; // this is dependency injection

        public ProductsController(ILogger<ProductsController> logger, IProductRepository productContext)
        {
            _logger = logger;
            _productContext = productContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return Ok(await _productContext.GetProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _productContext.GetProductByIdAsync(id);
        }

        [HttpGet("ProductBrands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productContext.GetProductBrandsAsync());
        }

        [HttpGet("ProductBrands/{id}")]
        public async Task<ActionResult<ProductBrand>> GetProductBrand(int id)
        {
            return await _productContext.GetProductBrandByIdAsync(id);
        }

        [HttpGet("ProductTypes")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            return Ok(await _productContext.GetProductTypesAsync());
        }

        [HttpGet("ProductTypes/{id}")]
        public async Task<ActionResult<ProductType>> GetProductType(int id)
        {
            return await _productContext.GetProductTypeByIdAsync(id);
        }
    }
}