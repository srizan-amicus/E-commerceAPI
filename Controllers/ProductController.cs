using System.Security.Claims;
using Asp.Versioning;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Customer")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<List<ProductDto>>> GetProducts(CancellationToken cancellationToken)
        {
            var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var products = await _productService.GetAllAsync(customerId, cancellationToken);

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
        public async Task<ActionResult<List<ProductPagedResponseDto>>> SearchProducts(
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


        //File management
        [HttpPost("{id}/image")]
        public async Task<IActionResult> UploadImage(
     int id,
     IFormFile file,
     CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Only JPG, PNG, and WEBP images are allowed.");

            const long maxFileSize = 5 * 1024 * 1024;

            if (file.Length > maxFileSize)
                return BadRequest("File size cannot exceed 5 MB.");

            var product = await _productService.GetByIdAsync(
                id,
                cancellationToken);

            if (product == null)
                return NotFound("Product not found.");

            var folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "product-images");

            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream, cancellationToken);

            var imagePath = $"product-images/{fileName}";

            await _productService.UpdateImagePathAsync(
                id,
                imagePath,
                cancellationToken);

            return Ok(new
            {
                Message = "Image uploaded successfully.",
                FileName = fileName
            });
        }
        //Download Image
        [HttpGet("{id}/image")]
        public async Task<IActionResult> DownloadImage(
    int id,
    CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(
                id,
                cancellationToken);

            if (product == null || string.IsNullOrEmpty(product.ImagePath))
                return NotFound("Product image not found.");

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                product.ImagePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Image file not found.");

            var contentType = Path.GetExtension(filePath).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

            var fileBytes = await System.IO.File.ReadAllBytesAsync(
                filePath,
                cancellationToken);

            return File(fileBytes, contentType);
        }
    }
}