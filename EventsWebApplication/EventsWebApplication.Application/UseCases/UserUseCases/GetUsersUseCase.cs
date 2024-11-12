using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class GetUsersUseCase(IMapper mapper, IUnitOfWork unitOfWork) : IGetUsersUseCase
    {
        public async Task<UserDto[]> Execute(CancellationToken cancellationToken)
        {
            var dtoarr = await unitOfWork.UserRepository
                .GetAsQueryable()
                .Select(dto => mapper.Map<UserDto>(dto))
                .ToArrayAsync(cancellationToken);
            return dtoarr;
        }
    }
}
