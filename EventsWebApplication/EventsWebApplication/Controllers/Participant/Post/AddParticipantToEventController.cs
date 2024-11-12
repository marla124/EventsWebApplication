using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Participant.Post
{
    [Route("api/Participant")]
    [ApiController]
    public class AddParticipantToEventController(IAddParticipantToEventUseCase addParticipantToEventUseCase) 
        : BaseController
    {
        [HttpPost("[action]/{eventId}")]
        [Authorize]
        public async Task<IActionResult> AddParticipantToEvent(Guid eventId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await addParticipantToEventUseCase.Execute(userId, eventId, cancellationToken);
            return Ok();
        }
    }
}
