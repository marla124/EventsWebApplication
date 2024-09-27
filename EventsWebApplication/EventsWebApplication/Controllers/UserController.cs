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
            var project = mapper.Map<UserViewModel>(await userService.GetById(id, cancellationToken));
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var projects = mapper.Map<List<UserViewModel>>(await userService.GetMany(cancellationToken));
            return Ok(projects);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            await userService.DeleteById(id, cancellationToken);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser(RegisterModel request, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var dto = mapper.Map<UserDto>(request);
                var user = await userService.RegisterUser(dto, cancellationToken);
                return Created($"users/{user.Id}", user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequestViewModel request, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var dto = mapper.Map<UserDto>(request);

                return Ok(mapper.Map<UserViewModel>(await userService.Update(dto, cancellationToken)));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
