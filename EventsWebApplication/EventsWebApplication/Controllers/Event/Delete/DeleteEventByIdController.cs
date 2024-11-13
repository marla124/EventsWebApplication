using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Event.Delete
{
    [Route("api/Event")]
    [ApiController]
    public class DeleteEventByIdController(IDeleteEventByIdUseCase deleteEventByIdUseCase) : BaseController
    {
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await deleteEventByIdUseCase.Execute(id, cancellationToken);
            return Ok();
        }
    }
}
