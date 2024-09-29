using AutoMapper;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    public class PracticantController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public PracticantController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddParticipantToEvent(Guid eventId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await _eventService.AddParticipantToEvent(userId, eventId, cancellationToken);
            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteParticipantFromEvent(Guid eventId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await _eventService.DeleteParticipantFromEvent(userId, eventId, cancellationToken);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventParticipants(Guid eventId, CancellationToken cancellationToken)
        {
            var practicants = await _eventService.GetEventParticipants(eventId, cancellationToken);
            return Ok(_mapper.Map<List<UserViewModel>>(practicants));
        }

        [HttpGet("[action]/{eventId, userId}")]
        public async Task<IActionResult> GetEventParticipantById(Guid eventId, Guid userId, CancellationToken cancellationToken)
        {
            var practicant = await _eventService.GetEventParticipantById(eventId, userId, cancellationToken);
            return Ok(practicant);
        }
    }
}
