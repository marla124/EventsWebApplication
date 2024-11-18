using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.PasswordUseCases;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.UseCases.AuthUseCases
{
    public class RegisterUserUseCase(IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService) : IRegisterUserUseCase
    {
        public async Task<UserDto> Execute(UserDto dto, CancellationToken cancellationToken)
        {
            var userRole = await unitOfWork.UserRoleRepository
                .FindBy(role => role.Role.Equals("User"))
                .FirstOrDefaultAsync(cancellationToken);

            if (userRole == null)
            {
                throw new KeyNotFoundException("User role not found");
            }
            var existingUser = await unitOfWork.UserRepository.GetByEmail(dto.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists");
            }
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                PasswordHash = passwordService.MdHashGenerate(dto.Password),
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UserRoleId = userRole.Id,
            };

            await unitOfWork.UserRepository.CreateOne(user, cancellationToken);
            await unitOfWork.UserRepository.Commit(cancellationToken);

            return mapper.Map<UserDto>(user);
        }
    }
}