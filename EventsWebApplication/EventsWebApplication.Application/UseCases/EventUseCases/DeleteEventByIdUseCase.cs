using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class DeleteEventByIdUseCase(IUnitOfWork unitOfWork) : IDeleteEventByIdUseCase
    {

        public async Task Execute(Guid eventId, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.EventRepository.GetById(eventId, cancellationToken);

            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            await unitOfWork.EventRepository.DeleteById(eventEntity, cancellationToken);
            await unitOfWork.EventRepository.Commit(cancellationToken);
        }
    }
}