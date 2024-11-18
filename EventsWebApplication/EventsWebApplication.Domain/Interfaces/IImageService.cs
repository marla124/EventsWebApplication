using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Domain.Interfaces;
public interface IImageService
{
    Task<byte[]> ConvertImageToByteArrayAsync(IFormFile file, CancellationToken cancellationToken);
}
