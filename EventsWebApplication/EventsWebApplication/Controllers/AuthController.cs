using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.AuthUseCases;
using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController(ILoginUseCase loginUseCase, IRemoveRefreshTokenUseCase _removeRefreshTokenUseCase,
        IUpdateRefreshTokenUseCase updateRefreshTokenUseCase, IRegisterUserUseCase registerUserUseCase, IMapper mapper) : ControllerBase
    {
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel, CancellationToken cancellationToken)
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var jwtToken = await loginUseCase.Execute(loginModel.Email, userAgent, cancellationToken);
            return Ok(new TokenResponseModel { JwtToken = jwtToken });
        }

        [HttpDelete]
        [Route("Revoke/{refreshToken}")]
        public async Task<IActionResult> RevokeToken(Guid refreshToken, CancellationToken cancellationToken)
        {
            await _removeRefreshTokenUseCase.Execute(refreshToken, cancellationToken);
            return Ok();
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel request, CancellationToken cancellationToken)
        {
            var guidRefToken = Guid.Parse(request.RefreshToken);
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var result = await updateRefreshTokenUseCase.Execute(guidRefToken, userAgent, cancellationToken);
            return Ok(result);
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel request, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<UserDto>(request);
            var user = mapper.Map<UserViewModel>(await registerUserUseCase.Execute(dto, cancellationToken));
            return Created($"users/{user.Id}", user);
        }
    }
}
