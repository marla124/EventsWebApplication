using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGetUserRoleUseCase _getUserRoleUseCase;
        public UpdateEventUseCase(IUnitOfWork unitOfWork, IGetUserRoleUseCase getUserRoleUseCase)
        {
            _unitOfWork = unitOfWork;
            _getUserRoleUseCase = getUserRoleUseCase;
        }
        public async Task<UpdateEventDto> Execute(UpdateEventDto dto, Guid userId, CancellationToken cancellationToken)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetById(dto.Id, cancellationToken);

            if (existingEvent == null)
            {
                throw new KeyNotFoundException();
            }   

            existingEvent.Name = dto.Name ?? existingEvent.Name;
            existingEvent.Description = dto.Description ?? existingEvent.Description;
            existingEvent.DateAndTime = dto.DateAndTime ?? existingEvent.DateAndTime;
            existingEvent.MaxNumberOfPeople = dto.MaxNumberOfPeople ?? existingEvent.MaxNumberOfPeople;
            existingEvent.Address = dto.Address ?? existingEvent.Address;
            existingEvent.CategoryId = dto.CategoryId ?? existingEvent.CategoryId;
            existingEvent.Image = dto.Image ?? existingEvent.Image;
            existingEvent.UpdatedAt = DateTime.UtcNow;
            try
            {
                await _unitOfWork.EventRepository.Update(existingEvent, cancellationToken);
                await _unitOfWork.EventRepository.Commit(cancellationToken);
                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

    }
}
