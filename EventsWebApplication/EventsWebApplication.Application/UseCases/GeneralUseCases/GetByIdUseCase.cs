using AutoMapper;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.GeneralUseCases
{
    public class GetByIdUseCase<TDto, TEntity> where TEntity : BaseEntity
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GetByIdUseCase(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto?> Execute(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(id, cancellationToken);

            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            var dto = _mapper.Map<TDto>(entity);
            return dto;
        }
    }
}