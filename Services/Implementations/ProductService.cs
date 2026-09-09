using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Services.Interfaces;

namespace EcommerceAPI.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            IMemoryCache cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<ProductDto>> GetAllAsync(
    int customerId,
    CancellationToken cancellationToken)
        {
            var cacheKey = $"products:{customerId}";

            if (_cache.TryGetValue(
                cacheKey,
                out List<ProductDto>? cachedProducts))
            {
                return cachedProducts!;
            }

            var products = await _productRepository.GetAllAsync(
                cancellationToken);

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            _cache.Set(
                cacheKey,
                productDtos,
                TimeSpan.FromHours(2));

            return productDtos;
        }

        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (product == null)
            {
                return null;
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductPagedResponseDto> SearchAsync(
            ProductQueryDto query, CancellationToken cancellationToken)
        {
            var result = await _productRepository.SearchAsync(
                query.Search,
                query.CategoryId,
                query.BrandId,
                query.MinPrice,
                query.MaxPrice,
                query.MinRating,
                query.SortBy,
                query.SortOrder,
                query.Page,
                query.PageSize,
                cancellationToken);
            return new ProductPagedResponseDto
            {
                Products = _mapper.Map<List<ProductDto>>(result.Products),
                Page = result.Page,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords,
                TotalPages = result.TotalPages
            };
        }


        public async Task<int> CreateAsync(Product product, CancellationToken cancellationToken)
        {
            return await _productRepository.CreateAsync(product, cancellationToken);
        }

        public async Task<bool> UpdateAsync(int id, Product product, CancellationToken cancellationToken)
        {
            return await _productRepository.UpdateAsync(id, product, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return await _productRepository.DeleteAsync(id,cancellationToken);
        }

        public async Task<bool> UpdateImagePathAsync(
    int id,
    string imagePath,
    CancellationToken cancellationToken)
        {
            return await _productRepository.UpdateImagePathAsync(
                id,
                imagePath,
                cancellationToken);
        }
    }
}