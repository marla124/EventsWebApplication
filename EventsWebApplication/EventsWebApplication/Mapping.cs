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
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<RegisterModel, UserDto>().ReverseMap();
            CreateMap<LoginModel, UserDto>().ReverseMap();
            CreateMap<UserDto, UserViewModel>().ReverseMap();
            CreateMap<UserDto, RegisterModel>().ReverseMap();
            CreateMap<UserDto, UpdateUserRequestViewModel>().ReverseMap();
            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
        }
    }
}
