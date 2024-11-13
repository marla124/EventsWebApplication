using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

public class UploadImageUseCase(IMapper mapper, IGetEventByIdUseCase getEventByIdUseCase, IImageService imageService, IUpdateEventUseCase updateEventUseCase) 
    : IUploadImageUseCase
{
    public async Task Execute(Guid eventId, Guid userId, IFormFile file, CancellationToken cancellationToken)
    {
        var imageData = await imageService.ConvertImageToByteArrayAsync(file, cancellationToken);

        var eventToUpdate = mapper.Map<UpdateEventDto>(await getEventByIdUseCase.Execute(eventId, cancellationToken));
        if (eventToUpdate == null)
        {
            throw new KeyNotFoundException("Event not found");
        }
        eventToUpdate.Image = imageData;
        await updateEventUseCase.Execute(eventToUpdate, userId, cancellationToken);
    }
}
