using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Auth
{
    [ApiController]
    [Route("api/Auth")]
    public class LogoutController(IRemoveRefreshTokenUseCase _removeRefreshTokenUseCase) : Controller
    {
        [HttpDelete]
        [Route("Revoke/{refreshToken}")]
        public async Task<IActionResult> RevokeToken(Guid refreshToken, CancellationToken cancellationToken)
        {
            await _removeRefreshTokenUseCase.Execute(refreshToken, cancellationToken);
            return Ok();
        }
    }
}
