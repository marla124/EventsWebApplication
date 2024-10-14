using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Models;


namespace EventsWebApplication
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateEventDto, EventDto>().ReverseMap();
            CreateMap<UpdateEventDto, Event>().ReverseMap();
            CreateMap<UpdateEventDto, UpdateEventModel>().ReverseMap();
            CreateMap<UserRoleDto, UserRole>().ReverseMap();
            CreateMap<UserRoleDto, UserRoleModel>().ReverseMap();
            CreateMap<CategoryDto, CategoryModel>().ReverseMap();
            CreateMap<EventDto, EventModel>().ReverseMap();
            CreateMap<EventDto, UpdateEventModel>().ReverseMap();
            CreateMap<EventDto, Event>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserRoleDto, UserRole>().ReverseMap();
            CreateMap<RegisterModel, UserDto>().ReverseMap();
            CreateMap<LoginModel, UserDto>().ReverseMap();
            CreateMap<UserDto, UserViewModel>().ReverseMap();
            CreateMap<UserDto, RegisterModel>().ReverseMap();
            CreateMap<UserDto, UpdateUserRequestViewModel>().ReverseMap();
            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
        }
    }
}
