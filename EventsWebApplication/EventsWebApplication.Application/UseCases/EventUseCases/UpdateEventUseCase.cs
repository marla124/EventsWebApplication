using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public UpdateEventUseCase(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<UpdateEventDto> Execute(UpdateEventDto dto, Guid userId, IFormFile? file, CancellationToken cancellationToken)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetById(dto.Id, cancellationToken);

            if (existingEvent == null)
            {
                throw new KeyNotFoundException();
            }

            if (file != null)
            {
                var imageData = await _imageService.ConvertImageToByteArrayAsync(file, cancellationToken);
                dto.Image = imageData;
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
