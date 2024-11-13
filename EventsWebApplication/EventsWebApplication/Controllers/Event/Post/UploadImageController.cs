using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Event.Post
{
    [Route("api/Event")]
    [ApiController]
    public class UploadImageController(IUploadImageUseCase uploadImageUseCase) : BaseController
    {
        [HttpPost("UploadImage/{eventId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImage(Guid eventId, IFormFile file, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await uploadImageUseCase.Execute(eventId, userId, file, cancellationToken);

            return Ok();
        }
    }
}
