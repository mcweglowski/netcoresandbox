using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.City, s => s.MapFrom(r => r.Address.City))
                .ForMember(d => d.Street, s => s.MapFrom(r => r.Address.Street))
                .ForMember(d => d.PostalCode, s => s.MapFrom(r => r.Address.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(d => d.Address, 
                    s => s.MapFrom(dto => new Address() 
                        {PostalCode = dto.PostalCode, Street = dto.Street, City = dto.City}));
        }
    }
}