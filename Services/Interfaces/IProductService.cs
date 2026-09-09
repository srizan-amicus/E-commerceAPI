using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync(int customerId, CancellationToken cancellationToken);

        Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ProductPagedResponseDto> SearchAsync(ProductQueryDto query, CancellationToken cancellationToken);

        Task<int> CreateAsync(Product product, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(int id, Product product, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

        Task<bool> UpdateImagePathAsync(int id, string imagePath, CancellationToken cancellationToken);
    }
}