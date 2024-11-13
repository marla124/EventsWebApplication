using AutoMapper;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Event.Get
{
    [Route("api/Event")]
    [ApiController]
    public class GetEventByNameController(IGetEventByNameUseCase getEventByNameUseCase, IMapper mapper) : ControllerBase
    {
        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            var events = mapper.Map<EventModel>(await getEventByNameUseCase.Execute(name, cancellationToken));
            return Ok(events);
        }
    }
}
