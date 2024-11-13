using Azure.Core;
using EventsWebApplication.Application.UseCases.AuthUseCases;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Auth
{
    [ApiController]
    [Route("api/Auth")]
    public class LoginController(ILoginUseCase loginUseCase) : ControllerBase
    {
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel, CancellationToken cancellationToken)
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var jwtToken = await loginUseCase.Execute(loginModel.Email, userAgent, cancellationToken);
            return Ok(new TokenResponseModel { JwtToken = jwtToken });
        }
    }
}
