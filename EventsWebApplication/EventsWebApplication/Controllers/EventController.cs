using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.BL;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using EventsWebApplication.BL.Interfaces;

namespace EventsWebApplication.Controllers
{
    public class EventController : BaseController
    {
        private readonly IEventService eventService;
        private readonly IMapper mapper;

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var events = mapper.Map<EventModel>(await eventService.GetById(id, cancellationToken));
            if (events == null)
            {
                return NotFound();
            }
            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEvents(CancellationToken cancellationToken)
        {
            var events = mapper.Map<List<EventModel>>(await eventService.GetMany(cancellationToken));
            return Ok(events);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            await eventService.DeleteById(id, cancellationToken);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEvent(EventModel request, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<EventDto>(request);
            var eventInfo = await eventService.Create(dto, cancellationToken);
            return Created($"event/{eventInfo.Id}", eventInfo);

        }

        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> UpdateEvent(EventModel request, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<EventDto>(request);
            return Ok(mapper.Map<EventModel>(await eventService.Update(dto, cancellationToken)));

        }
    }
}
