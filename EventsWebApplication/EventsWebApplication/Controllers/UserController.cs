using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var users = mapper.Map<UserViewModel>(await userService.GetById(id, cancellationToken));
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = mapper.Map<List<UserViewModel>>(await userService.GetMany(cancellationToken));
            return Ok(users);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            await userService.DeleteById(id, cancellationToken);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser(RegisterModel request, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<UserDto>(request);
            var user = await userService.RegisterUser(dto, cancellationToken);
            return Created($"users/{user.Id}", user);

        }

        [HttpPatch]
        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequestViewModel request, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<UserDto>(request);
            return Ok(mapper.Map<UserViewModel>(await userService.Update(dto, cancellationToken)));

        }
    }
}
