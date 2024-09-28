using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EventsWebApplication.BL
{
    public class UserService : Service<UserDto, User>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserService(IMapper mapper,
            IUnitOfWork unitOfWork, IConfiguration configuration) : base(unitOfWork.UserRepository, mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterUser(UserDto dto, CancellationToken cancellationToken)
        {
            var userRole = await _unitOfWork.UserRoleRepository
            .FindBy(role => role.Role
            .Equals("User")).FirstOrDefaultAsync(cancellationToken);
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name= dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                PasswordHash = MdHashGenerate(dto.Password),
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UserRoleId = userRole.Id,
            };
            await _unitOfWork.UserRepository.CreateOne(user, cancellationToken);

            await _unitOfWork.UserRepository.Commit(cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
        private string MdHashGenerate(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                var salt = _configuration["AppSettings:PasswordSalt"];
                byte[] inputBytes = Encoding.UTF8.GetBytes($"{input}{salt}");
                byte[] HashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(HashBytes);
            }
        }
        public bool IsUserExists(string email)
        {
            return _unitOfWork.UserRepository
                .FindBy(user => user.Email.Equals(email)).Any();
        }

        public async Task<bool> CheckPasswordCorrect(string email, string password, CancellationToken cancellationToken)
        {
            var currentPasswordHash = (await _unitOfWork.UserRepository
                .FindBy(user => user.Email.Equals(email)).FirstOrDefaultAsync(cancellationToken))?.PasswordHash;
            var passwordHash = MdHashGenerate(password);
            return currentPasswordHash?.Equals(passwordHash) ?? false;
        }

        public async Task<UserDto> GetUserByRefreshToken(Guid refreshToken, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByRefreshToken(refreshToken, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(email, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserRoleDto> GetUserRole(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException(nameof(user));
            }
            var role = await _unitOfWork.UserRoleRepository.GetById(user.UserRoleId, cancellationToken);
            return _mapper.Map<UserRoleDto>(role);
        }

    }
}
