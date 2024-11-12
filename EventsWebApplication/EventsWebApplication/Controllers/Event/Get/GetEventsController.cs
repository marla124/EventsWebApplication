using AutoMapper;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Event.Get
{
    [Route("api/Event")]
    [ApiController]
    public class GetEventsController(IGetEventsUseCase getEventsUseCase, IMapper mapper) : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEvents(CancellationToken cancellationToken)
        {
            var events = mapper.Map<List<EventModel>>(await getEventsUseCase.Execute(cancellationToken));
            return Ok(events);
        }
    }
}
