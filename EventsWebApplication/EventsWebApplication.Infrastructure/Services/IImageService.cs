using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Infrastructure.Services;
public interface IImageService
{
    Task<byte[]> ConvertImageToByteArrayAsync(IFormFile file, CancellationToken cancellationToken);
}
