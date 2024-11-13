using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Event.Update
{
    [Route("api/Event")]
    [ApiController]
    public class UpdateEventController(IUpdateEventUseCase updateEventUseCase, IMapper mapper) : BaseController
    {
        [HttpPatch]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent(UpdateEventModel request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var dto = mapper.Map<UpdateEventDto>(request);
            return Ok(mapper.Map<UpdateEventModel>(await updateEventUseCase.Execute(dto, userId, cancellationToken)));

        }
    }
}
