using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.GeneralUseCases.Interface;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IGetUserRoleUseCase _getUserRoleUseCase;
        private readonly IDeleteUserByIdUseCase _deleteUserByIdUseCase;
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly IGetUserByIdUseCase _getUserByIdUseCase;
        private readonly IGetUsersUseCase _getUsersUseCase;

        public UserController(IMapper mapper, IGetUserRoleUseCase getUserRoleUseCase,
            IDeleteUserByIdUseCase deleteUserByIdUseCase, IRegisterUserUseCase registerUserUseCase,
            IGetUserByIdUseCase getUserByIdUseCase, IGetUsersUseCase getUsersUseCase)
        {
            _mapper = mapper;
            _getUserRoleUseCase = getUserRoleUseCase;
            _deleteUserByIdUseCase = deleteUserByIdUseCase;
            _registerUserUseCase = registerUserUseCase;
            _getUserByIdUseCase = getUserByIdUseCase;
            _getUsersUseCase = getUsersUseCase;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var users = _mapper.Map<UserViewModel>(await _getUserByIdUseCase.Execute(id, cancellationToken));

            return Ok(users);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserRole(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            var role = await _getUserRoleUseCase.Execute(userId, cancellationToken);
            return Ok(_mapper.Map<UserRoleModel>(role));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = _mapper.Map<List<UserViewModel>>(await _getUsersUseCase.Execute(cancellationToken));
            return Ok(users);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
        {
            await _deleteUserByIdUseCase.Execute(id, cancellationToken);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser(RegisterModel request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<UserDto>(request);
            var user = await _registerUserUseCase.Execute(dto, cancellationToken);
            return Created($"users/{user.Id}", user);

        }
    }

}

