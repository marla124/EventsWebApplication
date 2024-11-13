using AutoMapper;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.User.Get
{
    [Route("api/User")]
    [ApiController]
    public class GetUserByIdController(IGetUserByIdUseCase getUserByIdUseCase, IMapper mapper) : ControllerBase
    {
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var users = mapper.Map<UserViewModel>(await getUserByIdUseCase.Execute(id, cancellationToken));

            return Ok(users);
        }
    }
}
