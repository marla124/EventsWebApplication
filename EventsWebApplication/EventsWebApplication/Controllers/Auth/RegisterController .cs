using AutoMapper;
using Azure.Core;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.AuthUseCases;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.Auth
{
    [ApiController]
    [Route("api/Auth")]
    public class RegisterController(IRegisterUserUseCase registerUserUseCase, IMapper mapper) : ControllerBase
    {
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
