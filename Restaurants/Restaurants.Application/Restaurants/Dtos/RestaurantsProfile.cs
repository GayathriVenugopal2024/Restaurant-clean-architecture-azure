using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurants;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurants;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<UpdateRestaurantsCommand, Restaurant>();

        CreateMap<CreateRestaurantsCommand, Restaurant>()
            .ForMember(x => x.Address, opt => opt.MapFrom(src => new Address
            {
                City = src.City,
                Street = src.Street,
                PostalCode = src.PostalCode,
            }));
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(x => x.City, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(x => x.PostalCode, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
            .ForMember(x => x.Street, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(x => x.Dishes, opt => opt.MapFrom(src => src.Dishes));
            
    }
}
