using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class GetUserRoleUseCase : IGetUserRoleUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserRoleUseCase(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UserRoleDto> Execute(Guid userId, CancellationToken cancellationToken)
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
