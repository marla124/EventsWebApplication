using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class DeleteUserByIdUseCase : IDeleteUserByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            await _unitOfWork.UserRepository.DeleteById(user, cancellationToken);
            await _unitOfWork.UserRepository.Commit(cancellationToken);
        }
    }
}