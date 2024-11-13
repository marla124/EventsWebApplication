using AutoMapper;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers.User.Get
{
    [Route("api/User")]
    [ApiController]
    public class GetUserRoleController(IGetUserRoleUseCase getUserRoleUseCase, IMapper mapper) : BaseController
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserRole(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var role = await getUserRoleUseCase.Execute(userId, cancellationToken);
            return Ok(mapper.Map<UserRoleModel>(role));
        }

    }
}
