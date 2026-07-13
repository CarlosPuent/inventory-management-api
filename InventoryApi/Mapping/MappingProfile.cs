using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Models;

namespace InventoryApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

        CreateMap<Category, CategoryResponseDto>();
        CreateMap<Supplier, SupplierResponseDto>();
        CreateMap<AppUser, AppUserResponseDto>();
    }
}