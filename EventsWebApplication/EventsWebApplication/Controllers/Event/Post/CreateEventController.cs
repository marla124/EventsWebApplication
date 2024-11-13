using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Event.Post
{
    [Route("api/Event")]
    [ApiController]
    public class CreateEventController(ICreateEventUseCase createEventUseCase, IMapper mapper) : BaseController
    {
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
    }
}
