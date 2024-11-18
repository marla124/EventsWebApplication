using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/Event")]
    [ApiController]
    public class EventController(IDeleteEventByIdUseCase deleteEventByIdUseCase, IMapper mapper,
        IGetEventByIdUseCase getEventByIdUseCase, IGetEventByNameUseCase getEventByNameUseCase, IGetEventsByCriteriaUseCase getEventsByCriteriaUseCase,
        IGetUsersEventsUseCase getUsersEventsUseCase, ICreateEventUseCase createEventUseCase,
        IUpdateEventUseCase updateEventUseCase) : BaseController
    {
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await deleteEventByIdUseCase.Execute(id, cancellationToken);
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var events = mapper.Map<EventModel>(await getEventByIdUseCase.Execute(id, cancellationToken));
            return Ok(events);
        }

        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            var events = mapper.Map<EventModel>(await getEventByNameUseCase.Execute(name, cancellationToken));
            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId,
            CancellationToken cancellationToken)
        {
            var events = mapper.Map<List<EventModel>>(await getEventsByCriteriaUseCase.Execute(date, address, categoryId, cancellationToken));
            return Ok(events);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUsersEvents(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var events = mapper.Map<List<EventModel>>(await getUsersEventsUseCase.Execute(userId, cancellationToken));
            return Ok(events);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEvent(CreateEventModel request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var dto = mapper.Map<EventDto>(request);
            dto.UserCreatorId = userId;
            var eventInfo = mapper.Map<EventModel>(await createEventUseCase.Execute(dto, cancellationToken));
            return Created($"event/{eventInfo.Id}", eventInfo);
        }

            [HttpPost("UploadImage/{eventId}")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> UploadImage(Guid eventId, IFormFile file, CancellationToken cancellationToken)
            {
                var userId = Guid.Parse(GetUserId());
                var dto = new UpdateEventDto { Id = eventId };

                await updateEventUseCase.Execute(dto, userId, file, cancellationToken);

                return Ok();
            }

            [HttpPatch("UpdateEvent")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> UpdateEvent(UpdateEventModel request, CancellationToken cancellationToken)
            {
                var userId = Guid.Parse(GetUserId());
                var dto = mapper.Map<UpdateEventDto>(request);
                return Ok(mapper.Map<UpdateEventModel>(await updateEventUseCase.Execute(dto, userId, null, cancellationToken)));
            }
        

    }
}
