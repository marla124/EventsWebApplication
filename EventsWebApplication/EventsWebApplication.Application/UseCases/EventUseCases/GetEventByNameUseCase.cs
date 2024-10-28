using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetEventByNameUseCase : IGetEventByNameUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventByNameUseCase(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<EventDto> Execute(string name, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetByName(name, cancellationToken);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
