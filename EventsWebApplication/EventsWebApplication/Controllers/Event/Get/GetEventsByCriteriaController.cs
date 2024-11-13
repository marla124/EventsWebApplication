using AutoMapper;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Event.Get
{
    [Route("api/Event")]
    [ApiController]
    public class GetEventsByCriteriaController(IGetEventsByCriteriaUseCase getEventsByCriteriaUseCase, IMapper mapper) : Controller
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId,
            CancellationToken cancellationToken)
        {
            var events = mapper.Map<List<EventModel>>(await getEventsByCriteriaUseCase.Execute(date, address, categoryId, cancellationToken));
            return Ok(events);
        }
    }
}
