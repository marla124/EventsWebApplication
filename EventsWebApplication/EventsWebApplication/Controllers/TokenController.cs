using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IGenerateJwtTokenUseCase _generateJwtTokenUseCase;
        private readonly IAddRefreshTokenUseCase _addRefreshTokenUseCase;
        private readonly IGetUserByEmailUseCase _getUserByEmailUseCase;
        public readonly IGetUserByRefreshTokenUseCase _getUserByRefreshTokenUseCase;
        public readonly ICheckRefreshTokenUseCase _checkRefreshTokenUseCase;
        public readonly IRemoveRefreshTokenUseCase _removeRefreshTokenUseCase;
        public TokenController( IGenerateJwtTokenUseCase generateJwtTokenUseCase,
            IAddRefreshTokenUseCase addRefreshTokenUseCase, IGetUserByEmailUseCase getUserByEmailUseCase,
            IGetUserByRefreshTokenUseCase getUserByRefreshTokenUseCase, ICheckRefreshTokenUseCase checkRefreshTokenUseCase,
            IRemoveRefreshTokenUseCase removeRefreshTokenUseCase)
        {
            _generateJwtTokenUseCase = generateJwtTokenUseCase;
            _addRefreshTokenUseCase = addRefreshTokenUseCase;
            _getUserByEmailUseCase = getUserByEmailUseCase;
            _getUserByRefreshTokenUseCase = getUserByRefreshTokenUseCase;
            _checkRefreshTokenUseCase = checkRefreshTokenUseCase;
            _removeRefreshTokenUseCase = removeRefreshTokenUseCase;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> GenerateToken(LoginModel request, CancellationToken cancellationToken)
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var userDto = await _getUserByEmailUseCase.Execute(request.Email, cancellationToken);
            var jwtToken = await _generateJwtTokenUseCase.Execute(userDto, cancellationToken);
            var refreshToken = await _addRefreshTokenUseCase.Execute(userDto.Email, userAgent, userDto.Id, cancellationToken);
            return Ok(new TokenResponseModel { JwtToken = jwtToken, RefreshToken = refreshToken });
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> GenerateTokenByRefresh(RefreshTokenModel request, CancellationToken cancellationToken)
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var userDto = await _getUserByRefreshTokenUseCase.Execute(request.RefreshToken, cancellationToken);
            var jwtToken = await _generateJwtTokenUseCase.Execute(userDto, cancellationToken);
            var refreshToken = await _addRefreshTokenUseCase.Execute(userDto.Email, userAgent, userDto.Id, cancellationToken);
            await _removeRefreshTokenUseCase.Execute(request.RefreshToken, cancellationToken);
            return Ok(new TokenResponseModel { JwtToken = jwtToken, RefreshToken = refreshToken });
        }

        [HttpDelete]
        [Route("Revoke/{refreshToken}")]
        public async Task<IActionResult> RevokeToken(Guid refreshToken, CancellationToken cancellationToken)
        {
            await _removeRefreshTokenUseCase.Execute(refreshToken, cancellationToken);
            return Ok();
        }
    }
}
