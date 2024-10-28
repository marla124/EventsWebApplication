using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.PasswordUseCases;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordUseCase _passwordUseCase;

        public RegisterUserUseCase(IUnitOfWork unitOfWork, IMapper mapper, IPasswordUseCase passwordUseCase)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordUseCase = passwordUseCase;
        }

        public async Task<UserDto> Execute(UserDto dto, CancellationToken cancellationToken)
        {
            var userRole = await _unitOfWork.UserRoleRepository
                .FindBy(role => role.Role.Equals("User"))
                .FirstOrDefaultAsync(cancellationToken);

            if (userRole == null)
            {
                throw new KeyNotFoundException("User role not found");
            }

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                PasswordHash = _passwordUseCase.MdHashGenerate(dto.Password),
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UserRoleId = userRole.Id,
            };

            await _unitOfWork.UserRepository.CreateOne(user, cancellationToken);
            await _unitOfWork.UserRepository.Commit(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}