using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.User.Delete
{
    [Route("api/User")]
    [ApiController]
    public class DeleteUserByIdController(IDeleteUserByIdUseCase deleteUserByIdUseCase) : ControllerBase
    {
        [HttpDelete("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            await deleteUserByIdUseCase.Execute(id, cancellationToken);
            return Ok();
        }
    }
}
