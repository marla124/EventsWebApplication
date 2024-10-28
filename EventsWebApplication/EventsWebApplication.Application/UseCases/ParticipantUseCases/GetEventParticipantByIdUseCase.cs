using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class GetEventParticipantByIdUseCase : IGetEventParticipantByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventParticipantByIdUseCase(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> Execute(Guid eventId, Guid userId, CancellationToken cancellationToken)
        {
            var userEventConnection = await _unitOfWork.EventRepository.GetUserEventConnection(userId, eventId, cancellationToken);

            if (userEventConnection == null)
            {
                throw new KeyNotFoundException("User is not a participant of the event");
            }

            return _mapper.Map<UserDto>(userEventConnection.User);
        }
    }
}