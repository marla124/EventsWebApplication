using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetEventByIdUseCase(IMapper mapper, IUnitOfWork unitOfWork) : IGetEventByIdUseCase
    {
        public async Task<EventDto> Execute(Guid id, CancellationToken cancellationToken)
        {
            var entity = await unitOfWork.EventRepository.GetById(id, cancellationToken);

            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            var dto = mapper.Map<EventDto>(entity);
            return dto;
        }
    }
}
