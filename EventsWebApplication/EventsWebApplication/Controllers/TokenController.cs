using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public TokenController(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> GenerateToken(LoginModel request, CancellationToken cancellationToken)
        {
            var isUserCorrect = await _userService.CheckPasswordCorrect(request.Email, request.Password, cancellationToken);
            if (isUserCorrect)
            {
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                var userDto = await _userService.GetByEmail(request.Email, cancellationToken);
                var jwtToken = await _tokenService.GenerateJwtToken(userDto, cancellationToken);
                var refreshToken = await _tokenService.AddRefreshToken(userDto.Email, userAgent, userDto.Id, cancellationToken);
                return Ok(new TokenResponseModel { JwtToken = jwtToken, RefreshToken = refreshToken });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> GenerateTokenByRefresh(RefreshTokenModel request, CancellationToken cancellationToken)
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var isRefreshTokenValid = await _tokenService.CheckRefreshToken(request.RefreshToken, cancellationToken);
            if (isRefreshTokenValid)
            {
                var userDto = await _userService.GetUserByRefreshToken(request.RefreshToken, cancellationToken);
                var jwtToken = await _tokenService.GenerateJwtToken(userDto, cancellationToken);
                var refreshToken = await _tokenService.AddRefreshToken(userDto.Email, userAgent, userDto.Id, cancellationToken);
                await _tokenService.RemoveRefreshToken(request.RefreshToken, cancellationToken);
                return Ok(new TokenResponseModel { JwtToken = jwtToken, RefreshToken = refreshToken });
            }

            return Unauthorized();
        }

        [HttpDelete]
        [Route("Revoke/{refreshToken}")]
        public async Task<IActionResult> RevokeToken(Guid refreshToken, CancellationToken cancellationToken)
        {
            var isRefreshTokenValid = await _tokenService.CheckRefreshToken(refreshToken, cancellationToken);
            if (isRefreshTokenValid)
            {
                await _tokenService.RemoveRefreshToken(refreshToken, cancellationToken);
                return Ok();
            }
            return Unauthorized();
        }
    }
}
