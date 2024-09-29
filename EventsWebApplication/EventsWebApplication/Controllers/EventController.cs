using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using EventsWebApplication.BL.Interfaces;

namespace EventsWebApplication.Controllers
{
    public class EventController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventController(IEventService eventService, IMapper mapper)
        {
            _eventService =eventService;
            _mapper = mapper;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var events = _mapper.Map<EventModel>(await _eventService.GetById(id, cancellationToken));
            if (events == null)
            {
                return NotFound();
            }
            return Ok(events);
        }

        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            var events = _mapper.Map<EventModel>(await _eventService.GetByName(name, cancellationToken));
            if (events == null)
            {
                return NotFound();
            }
            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEvents(CancellationToken cancellationToken)
        {
            var events = _mapper.Map<List<EventModel>>(await _eventService.GetMany(cancellationToken));
            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsByCriteria(DateTime? date, string? address, string? categoryName,
            CancellationToken cancellationToken)
        {
            var events = _mapper.Map<List<EventModel>>(await _eventService.GetEventsByCriteria(date, address, categoryName, cancellationToken));
            return Ok(events);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await _eventService.DeleteById(id, userId, cancellationToken);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEvent(EventModel request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var dto = _mapper.Map<EventDto>(request);
            dto.UserCreatorId = userId;
            var eventInfo = await _eventService.Create(dto, cancellationToken);
            return Created($"event/{eventInfo.Id}", eventInfo);

        }

        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> UpdateEvent(UpdateEventModel request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<EventDto>(request);
            return Ok(_mapper.Map<UpdateEventModel>(await _eventService.Update(dto, cancellationToken)));

        }
    }
}
