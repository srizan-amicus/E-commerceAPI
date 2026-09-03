using AutoMapper;
using EcommerceAPI.DTO;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
               
            // Brand
            CreateMap<Brand, BrandDto>();
            CreateMap<BrandDto, Brand>();

            // Product
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<ProductDetails, ProductDto>();

            // Product Price
            CreateMap<ProductPrice, ProductPriceDto>();
            CreateMap<CreateProductPriceDto, ProductPrice>();

            // Product Availability
            CreateMap<ProductAvailability, ProductAvailabilityDto>();
            CreateMap<InventoryCheckItemDto, InventoryCheckItem>();
            CreateMap<InventoryCheckResult, InventoryCheckResultDto>();

            //cart
            CreateMap<CartItem, CartItemDto>();
        }
    }
}