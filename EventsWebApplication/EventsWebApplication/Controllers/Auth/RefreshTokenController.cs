using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Auth")]
public class RefreshTokenController(IUpdateRefreshTokenUseCase updateRefreshTokenUseCase) : ControllerBase
{
    [HttpPost("Refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenModel request, CancellationToken cancellationToken)
    {
        var guidRefToken = Guid.Parse(request.RefreshToken);
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        var result = await updateRefreshTokenUseCase.Execute(guidRefToken, userAgent, cancellationToken);
        return Ok(result);
    }
}
