using AutoMapper;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.UseCases.GeneralUseCases
{
    public class GetManyUseCase<TDto, TEntity> where TEntity : BaseEntity
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GetManyUseCase(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto[]?> Execute(CancellationToken cancellationToken)
        {
            var dtoarr = await _repository
                .GetAsQueryable()
                .Select(dto => _mapper.Map<TDto>(dto))
                .ToArrayAsync(cancellationToken);
            return dtoarr;
        }
    }
}
