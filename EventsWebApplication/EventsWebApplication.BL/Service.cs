using AutoMapper;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.BL
{
    public class Service<TDto, TEntity> where TEntity : BaseEntity
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public Service(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto?> GetById(Guid Id, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<TDto>(await _repository.GetById(Id, cancellationToken));
            return dto;
        }

        public async Task<TDto[]?> GetMany(CancellationToken cancellationToken)
        {
            var dtoarr = await _repository
            .GetAsQueryable()
            .Select(dto => _mapper.Map<TDto>(dto))
            .ToArrayAsync(cancellationToken);
            return dtoarr;
        }

        public async Task DeleteById(Guid Id, CancellationToken cancellationToken)
        {
            await _repository.DeleteById(Id, cancellationToken);
            await _repository.Commit(cancellationToken);
        }

        public async Task<TDto?> Create(TDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var createdEntity = await _repository.CreateOne(entity, cancellationToken);
            await _repository.Commit(cancellationToken);
            return _mapper.Map<TDto>(createdEntity);
        }

        public async Task<TDto?> Update(TDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var createdEntity = await _repository.Update(entity, cancellationToken);
            await _repository.Commit(cancellationToken);
            return _mapper.Map<TDto>(createdEntity);
        }
    }
}
