using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class GetEventParticipantsUseCase : IGetEventParticipantsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventParticipantsUseCase(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDto>?> Execute(Guid eventId, CancellationToken cancellationToken)
        {
            var eventExists = await _unitOfWork.EventRepository.GetById(eventId, cancellationToken);
            if (eventExists == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            var users = await _unitOfWork.EventRepository.GetEventParticipants(eventId, cancellationToken);
            return _mapper.Map<List<UserDto>?>(users);
        }
    }
}