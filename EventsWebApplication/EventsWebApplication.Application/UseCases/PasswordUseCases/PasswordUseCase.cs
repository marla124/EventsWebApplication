using System.Security.Cryptography;
using System.Text;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventsWebApplication.Application.UseCases.PasswordUseCases
{
    public class PasswordUseCase : IPasswordUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public PasswordUseCase(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<bool> CheckPasswordCorrect(string email, string password, CancellationToken cancellationToken)
        {
            var currentPasswordHash = (await _unitOfWork.UserRepository
                .FindBy(user => user.Email.Equals(email)).FirstOrDefaultAsync(cancellationToken))?.PasswordHash;
            var passwordHash = MdHashGenerate(password);
            return currentPasswordHash?.Equals(passwordHash) ?? false;
        }
        public string MdHashGenerate(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                var salt = _configuration["AppSettings:PasswordSalt"];
                byte[] inputBytes = Encoding.UTF8.GetBytes($"{input}{salt}");
                byte[] HashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(HashBytes);
            }
        }
    }
}
