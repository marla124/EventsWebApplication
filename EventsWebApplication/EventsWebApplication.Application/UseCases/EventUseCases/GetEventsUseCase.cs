using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetEventsUseCase(IMapper mapper, IUnitOfWork unitOfWork) : IGetEventsUseCase
    {
        public async Task<EventDto[]> Execute(CancellationToken cancellationToken)
        {
            var dtoarr = await unitOfWork.EventRepository
                .GetAsQueryable()
                .Select(dto => mapper.Map<EventDto>(dto))
                .ToArrayAsync(cancellationToken);
            return dtoarr;
        }
    }
}
