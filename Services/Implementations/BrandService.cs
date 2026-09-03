using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Services.Interfaces;

namespace EcommerceAPI.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(
            IBrandRepository brandRepository,
            IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            var brands =
                await _brandRepository.GetAllAsync();

            return _mapper.Map<List<BrandDto>>(brands);
        }

        public async Task<BrandDto?> GetByIdAsync(int id)
        {
            var brand =
                await _brandRepository.GetByIdAsync(id);

            if (brand == null)
            {
                return null;
            }

            return _mapper.Map<BrandDto>(brand);
        }
    }
}