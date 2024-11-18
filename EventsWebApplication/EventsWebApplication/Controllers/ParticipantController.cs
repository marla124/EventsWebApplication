using AutoMapper;
using EventsWebApplication.Application.UseCases.ParticipantUseCases;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/Participant")]
    [ApiController]
    public class ParticipantController(IDeleteParticipantFromEventUseCase deleteParticipantFromEventUseCase,
        IGetEventParticipantByIdUseCase getEventParticipantByIdUseCase, IGetEventParticipantsUseCase getEventParticipantsUseCase,
        IMapper mapper, IAddParticipantToEventUseCase addParticipantToEventUseCase)
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

        [HttpGet("[action]/{eventId, userId}")]
        public async Task<IActionResult> GetEventParticipantById(Guid eventId, Guid userId, CancellationToken cancellationToken)
        {
            var practicant = await getEventParticipantByIdUseCase.Execute(eventId, userId, cancellationToken);
            return Ok(mapper.Map<UserViewModel>(practicant));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventParticipants(Guid eventId, CancellationToken cancellationToken)
        {
            var practicants = await getEventParticipantsUseCase.Execute(eventId, cancellationToken);
            return Ok(mapper.Map<List<UserViewModel>>(practicants));
        }

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

