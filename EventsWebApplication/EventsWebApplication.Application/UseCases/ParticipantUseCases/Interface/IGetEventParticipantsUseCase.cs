using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;

public interface IGetEventParticipantsUseCase
{
    public Task<List<UserDto>?> Execute(Guid eventId, CancellationToken cancellationToken);

}