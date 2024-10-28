using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.GeneralUseCases;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class GetUserByIdUseCase : GetByIdUseCase<UserDto, User>, IGetUserByIdUseCase
    {
        public GetUserByIdUseCase(IUserRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
