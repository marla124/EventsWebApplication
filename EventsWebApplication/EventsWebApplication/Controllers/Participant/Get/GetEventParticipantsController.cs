using AutoMapper;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Participant.Get
{
    [Route("api/Participant")]
    [ApiController]
    public class GetEventParticipantsController(IGetEventParticipantsUseCase getEventParticipantsUseCase, 
        IMapper mapper) : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventParticipants(Guid eventId, CancellationToken cancellationToken)
        {
            var practicants = await getEventParticipantsUseCase.Execute(eventId, cancellationToken);
            return Ok(mapper.Map<List<UserViewModel>>(practicants));
        }

    }
}
