using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken);

        Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<List<ProductDto>> SearchAsync(ProductQueryDto query, CancellationToken cancellationToken);

        Task<int> CreateAsync(Product product, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(int id, Product product, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}