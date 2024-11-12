using AutoMapper;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Participant.Get
{
    [Route("api/Participant")]
    [ApiController]
    public class GetEventParticipantByIdController(IGetEventParticipantByIdUseCase getEventParticipantByIdUseCase, 
        IMapper mapper) : ControllerBase
    {
        [HttpGet("[action]/{eventId, userId}")]
        public async Task<IActionResult> GetEventParticipantById(Guid eventId, Guid userId, CancellationToken cancellationToken)
        {
            var practicant = await getEventParticipantByIdUseCase.Execute(eventId, userId, cancellationToken);
            return Ok(mapper.Map<UserViewModel>(practicant));
        }
    }
}
