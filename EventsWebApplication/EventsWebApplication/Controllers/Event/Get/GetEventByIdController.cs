using AutoMapper;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Event.Get
{
    [Route("api/Event")]
    [ApiController]
    public class GetEventByIdController(IGetEventByIdUseCase getEventByIdUseCase, IMapper mapper) : ControllerBase
    {
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var events = mapper.Map<EventModel>(await getEventByIdUseCase.Execute(id, cancellationToken));
            return Ok(events);
        }
    }
}
