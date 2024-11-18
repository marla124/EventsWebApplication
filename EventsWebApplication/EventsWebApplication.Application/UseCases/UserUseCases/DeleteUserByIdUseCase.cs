using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class DeleteUserByIdUseCase(IUnitOfWork unitOfWork) : IDeleteUserByIdUseCase
    {
        public async Task Execute(Guid userId, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            await unitOfWork.UserRepository.DeleteById(user, cancellationToken);
            await unitOfWork.UserRepository.Commit(cancellationToken);
        }
    }
}