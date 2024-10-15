using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using EventsWebApplication.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace EventsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public EventController(IEventService eventService, IMapper mapper, ICategoryService categoryService)
        {
            _eventService =eventService;
            _mapper = mapper;
            _categoryService = categoryService;
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
        [Authorize]
        public async Task<IActionResult> GetUsersEvents(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var events = _mapper.Map<List<EventModel>>(await _eventService.GetUsersEvents(userId, cancellationToken));
            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategory(CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetMany(cancellationToken);
            var categoryMap = _mapper.Map<List<CategoryModel>>(category);
            return Ok(categoryMap);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId,
            CancellationToken cancellationToken)
        {
            var events = _mapper.Map<List<EventModel>>(await _eventService.GetEventsByCriteria(date, address, categoryId, cancellationToken));
            return Ok(events);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await _eventService.DeleteById(id, userId, cancellationToken);
            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
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
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent(UpdateEventModel request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var dto = _mapper.Map<UpdateEventDto>(request);
            return Ok(_mapper.Map<UpdateEventModel>(await _eventService.Update(dto, userId, cancellationToken)));

        }

        [HttpPost("UploadImage/{eventId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImage(Guid eventId, IFormFile file, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await _eventService.UploadImage(eventId, userId, file, cancellationToken);

            return Ok();
        }

    }
}
