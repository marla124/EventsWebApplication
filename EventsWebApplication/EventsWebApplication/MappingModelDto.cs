using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Models;


namespace EventsWebApplication
{
    public class MappingModelDto : Profile
    {
        public MappingModelDto()
        {
            CreateMap<EventDto, CreateEventModel>().ReverseMap();
            CreateMap<UpdateEventDto, UpdateEventModel>().ReverseMap();
            CreateMap<UserRoleDto, UserRoleModel>().ReverseMap();
            CreateMap<CategoryDto, CategoryModel>().ReverseMap();
            CreateMap<EventDto, EventModel>().ReverseMap();
            CreateMap<EventDto, UpdateEventModel>().ReverseMap();
            CreateMap<RegisterModel, UserDto>().ReverseMap();
            CreateMap<LoginModel, UserDto>().ReverseMap();
            CreateMap<UserDto, UserViewModel>().ReverseMap();
            CreateMap<UserDto, RegisterModel>().ReverseMap();
            CreateMap<UserDto, UpdateUserRequestViewModel>().ReverseMap();
        }
    }
}
