using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;

public interface IGetEventParticipantByIdUseCase
{
    public Task<UserDto> Execute(Guid eventId, Guid userId, CancellationToken cancellationToken);
}