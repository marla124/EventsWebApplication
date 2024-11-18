using EventsWebApplication.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Infrastructure.Services;
public class ImageService : IImageService
{
    public async Task<byte[]> ConvertImageToByteArrayAsync(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            throw new InvalidOperationException("The file is empty or has not been provided.");
        }

        byte[] imageData;
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream, cancellationToken);
            imageData = memoryStream.ToArray();
        }

        return imageData;
    }
}
