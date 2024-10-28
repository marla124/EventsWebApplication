using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;

public interface IUploadImageUseCase
{
    public Task Execute(Guid eventId, Guid userId, IFormFile file, CancellationToken cancellationToken);
}