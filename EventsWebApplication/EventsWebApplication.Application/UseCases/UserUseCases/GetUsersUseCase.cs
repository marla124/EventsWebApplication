using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.GeneralUseCases;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class GetUsersUseCase : GetManyUseCase<UserDto, User>, IGetUsersUseCase
    {
        public GetUsersUseCase(IUserRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
