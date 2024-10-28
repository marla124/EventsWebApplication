using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetUsersEventsUseCase : IGetUsersEventsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersEventsUseCase(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EventDto>?> Execute(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var events = await _unitOfWork.EventRepository.GetUsersEvents(userId, cancellationToken);
            return _mapper.Map<List<EventDto>?>(events);
        }
    }
}