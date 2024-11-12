using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class DeleteEventByIdUseCase : IDeleteEventByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(Guid eventId, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetById(eventId, cancellationToken);

            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            await _unitOfWork.EventRepository.DeleteById(eventEntity, cancellationToken);
            await _unitOfWork.EventRepository.Commit(cancellationToken);
        }
    }
}