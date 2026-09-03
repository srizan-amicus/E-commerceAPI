using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync(cancellationToken);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id,
            CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(id, cancellationToken);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ProductDto>>> SearchProducts(
            [FromQuery] ProductQueryDto query, CancellationToken cancellationToken)
        {
            var products = await _productService.SearchAsync(query, cancellationToken);

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, CancellationToken cancellationToken)
        {
            var id = await _productService.CreateAsync(product, cancellationToken);

            return CreatedAtAction(
                nameof(GetProduct),
                new { id },
                new { ProductId = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
    int id,
    Product product, CancellationToken cancellationToken)
        {
            var updated = await _productService.UpdateAsync(id, product,cancellationToken);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _productService.DeleteAsync(id, cancellationToken);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("This is a test exception.");
        }


    }
}