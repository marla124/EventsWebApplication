using AutoMapper;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.CategoryUseCase;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Application.UseCases.GeneralUseCases.Interface;

namespace EventsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : BaseController
    {
        private readonly ICreateEventUseCase _createEventUseCase;
        private readonly IDeleteEventByIdUseCase _deleteEventByIdUseCase;
        private readonly IUpdateEventUseCase _updateEventUseCase;
        private readonly IGetEventsUseCase _getEventsUseCase;
        private readonly IGetEventByIdUseCase _getEventByIdUseCase;
        private readonly IGetEventByNameUseCase _getEventByNameUseCase;
        private readonly IGetEventsByCriteriaUseCase _getEventsByCriteriaUseCase;
        private readonly IGetCategoriesUseCase _getCategoriesUseCase;
        private readonly IGetUsersEventsUseCase _getUsersEventsUseCase;
        private readonly IUploadImageUseCase _uploadImageUseCase;
        private readonly IMapper _mapper;

        public EventController(IMapper mapper, IGetCategoriesUseCase getCategoriesUseCase, IGetEventByIdUseCase getEventByIdUseCase, 
            IGetEventsByCriteriaUseCase getEventsByCriteriaUseCase, IUpdateEventUseCase updateEventUseCase, 
            IDeleteEventByIdUseCase deleteEventByIdUseCase, ICreateEventUseCase createEventUseCase, 
            IGetEventByNameUseCase getEventByNameUseCase, IGetEventsUseCase getEventsUseCase, IGetUsersEventsUseCase getUsersEventsUseCase, IUploadImageUseCase uploadImageUseCase)
        {
            _mapper = mapper;
            _getCategoriesUseCase= getCategoriesUseCase;
            _getEventByIdUseCase = getEventByIdUseCase;
            _getEventsByCriteriaUseCase = getEventsByCriteriaUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventByIdUseCase = deleteEventByIdUseCase;
            _createEventUseCase = createEventUseCase;
            _getEventByNameUseCase = getEventByNameUseCase;
            _getEventsUseCase = getEventsUseCase;
            _getUsersEventsUseCase = getUsersEventsUseCase;
            _uploadImageUseCase = uploadImageUseCase;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var events = _mapper.Map<EventModel>(await _getEventByIdUseCase.Execute(id, cancellationToken));
            return Ok(events);
        }

        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            var events = _mapper.Map<EventModel>(await _getEventByNameUseCase.Execute(name, cancellationToken));
            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEvents(CancellationToken cancellationToken)
        {
            var events = _mapper.Map<List<EventModel>>(await _getEventsUseCase.Execute(cancellationToken));
            return Ok(events);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUsersEvents(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var events = _mapper.Map<List<EventModel>>(await _getUsersEventsUseCase.Execute(userId, cancellationToken));
            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategory(CancellationToken cancellationToken)
        {
            var category = await _getCategoriesUseCase.Execute(cancellationToken);
            var categoryMap = _mapper.Map<List<CategoryModel>>(category);
            return Ok(categoryMap);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId,
            CancellationToken cancellationToken)
        {
            var events = _mapper.Map<List<EventModel>>(await _getEventsByCriteriaUseCase.Execute(date, address, categoryId, cancellationToken));
            return Ok(events);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await _deleteEventByIdUseCase.Execute(id, userId, cancellationToken);
            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEvent(CreateEventModel request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var dto = _mapper.Map<EventDto>(request);
            dto.UserCreatorId = userId;
            var eventInfo = await _createEventUseCase.Execute(dto, cancellationToken);
            return Created($"event/{eventInfo.Id}", eventInfo);

        }

        [HttpPatch]
        [Route("[action]")]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent(UpdateEventModel request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var dto = _mapper.Map<UpdateEventDto>(request);
            return Ok(_mapper.Map<UpdateEventModel>(await _updateEventUseCase.Execute(dto, userId, cancellationToken)));

        }

        [HttpPost("UploadImage/{eventId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImage(Guid eventId, IFormFile file, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await _uploadImageUseCase.Execute(eventId, userId, file, cancellationToken);

            return Ok();
        }

    }
}
