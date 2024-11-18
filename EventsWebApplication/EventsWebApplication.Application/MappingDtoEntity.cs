using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Application
{
    public class MappingDtoEntity : Profile
    {
        public MappingDtoEntity()
        {
            CreateMap<UpdateEventDto, EventDto>().ReverseMap();
            CreateMap<UpdateEventDto, Event>().ReverseMap();
            CreateMap<UserRoleDto, UserRole>().ReverseMap();
            CreateMap<EventDto, Event>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserRoleDto, UserRole>().ReverseMap();
            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
        }
    }
}
