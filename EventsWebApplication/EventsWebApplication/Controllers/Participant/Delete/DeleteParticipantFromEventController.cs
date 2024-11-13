using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Participant.Delete
{
    [Route("api/Participant")]
    [ApiController]
    public class DeleteParticipantFromEventController(IDeleteParticipantFromEventUseCase deleteParticipantFromEventUseCase) 
        : BaseController
    {
        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> DeleteParticipantFromEvent(Guid eventId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await deleteParticipantFromEventUseCase.Execute(userId, eventId, cancellationToken);
            return Ok();
        }
    }
}
