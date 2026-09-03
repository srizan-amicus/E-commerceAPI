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

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<List<ProductDto>>(products);
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

        public async Task<List<ProductDto>> SearchAsync(
            ProductQueryDto query, CancellationToken cancaellationToken)
        {
            var products = await _productRepository.SearchAsync(
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
                cancaellationToken);

            return _mapper.Map<List<ProductDto>>(products);
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
    }
}