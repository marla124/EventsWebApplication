using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetUsersEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper) : IGetUsersEventsUseCase
    {
        public async Task<List<EventDto>?> Execute(Guid userId, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var events = await unitOfWork.EventRepository.GetUsersEvents(userId, cancellationToken);
            return mapper.Map<List<EventDto>?>(events);
        }
    }
}