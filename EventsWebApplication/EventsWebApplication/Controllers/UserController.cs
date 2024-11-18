using AutoMapper;
using EventsWebApplication.Application.UseCases.UserUseCases;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController(IDeleteUserByIdUseCase deleteUserByIdUseCase, IGetUserByIdUseCase getUserByIdUseCase, IMapper mapper,
        IGetUserRoleUseCase getUserRoleUseCase) : BaseController
    {
        [HttpDelete("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            await deleteUserByIdUseCase.Execute(id, cancellationToken);
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var users = mapper.Map<UserViewModel>(await getUserByIdUseCase.Execute(id, cancellationToken));

            return Ok(users);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserRole(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var role = await getUserRoleUseCase.Execute(userId, cancellationToken);
            return Ok(mapper.Map<UserRoleModel>(role));
        }
    }
}