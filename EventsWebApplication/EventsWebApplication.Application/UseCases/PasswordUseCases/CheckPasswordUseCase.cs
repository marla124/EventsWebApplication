using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.UseCases.PasswordUseCases
{
    public class CheckPasswordUseCase(IPasswordService passwordService, IUnitOfWork unitOfWork) : ICheckPasswordUseCase
    {
        public async Task<bool> CheckPasswordCorrect(string email, string password, CancellationToken cancellationToken)
        {
            var currentPasswordHash = (await unitOfWork.UserRepository
                .FindBy(user => user.Email.Equals(email)).FirstOrDefaultAsync(cancellationToken))?.PasswordHash;
            var passwordHash = passwordService.MdHashGenerate(password);
            return currentPasswordHash?.Equals(passwordHash) ?? false;
        }
        
    }
}
