using AutoMapper;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Event.Get
{
    [Route("api/Event")]
    [ApiController]
    public class GetUsersEventsController(IGetUsersEventsUseCase getUsersEventsUseCase, IMapper mapper) : BaseController
    {
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUsersEvents(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var events = mapper.Map<List<EventModel>>(await getUsersEventsUseCase.Execute(userId, cancellationToken));
            return Ok(events);
        }
    }
}
