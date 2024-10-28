using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Application.UseCases.GeneralUseCases;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetEventByIdUseCase : GetByIdUseCase<EventDto, Event>, IGetEventByIdUseCase
    {
        public GetEventByIdUseCase(IEventRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
