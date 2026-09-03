using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductDetails>> GetAllAsync(CancellationToken cancellationToken);

        Task<ProductDetails?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<List<ProductDetails>> SearchAsync(
            string? search,
            int? categoryId,
            int? brandId,
            decimal? minPrice,
            decimal? maxPrice,
            decimal? minRating,
            string? sortBy,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CreateAsync(Product product, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(int id, Product product, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}