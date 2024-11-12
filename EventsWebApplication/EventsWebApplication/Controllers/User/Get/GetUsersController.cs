using AutoMapper;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.User.Get
{
    [Route("api/User")]
    [ApiController]
    public class GetUsersController(IMapper mapper, IGetUsersUseCase getUsersUseCase) : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = mapper.Map<List<UserViewModel>>(await getUsersUseCase.Execute(cancellationToken));
            return Ok(users);
        }
    }
}
