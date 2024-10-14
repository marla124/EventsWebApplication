using AutoMapper;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticantController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public PracticantController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpPost("[action]/{eventId}")]
        [Authorize]
        public async Task<IActionResult> AddParticipantToEvent(Guid eventId, CancellationToken cancellationToken)
        {
            try
            {
                var userId = Guid.Parse(GetUserId());
                await _eventService.AddParticipantToEvent(userId, eventId, cancellationToken);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return Conflict();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("[action]")]
        [Authorize]
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
