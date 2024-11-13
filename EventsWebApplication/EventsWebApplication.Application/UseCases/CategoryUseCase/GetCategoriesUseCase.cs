using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.UseCases.CategoryUseCase
{
    public class GetCategoriesUseCase(IMapper mapper, IUnitOfWork unitOfWork) : IGetCategoriesUseCase
    {
        public async Task<CategoryDto[]?> Execute(CancellationToken cancellationToken)
        {
            var dtoarr = await unitOfWork.CategoryRepository
                .GetAsQueryable()
                .Select(dto => mapper.Map<CategoryDto>(dto))
                .ToArrayAsync(cancellationToken);
            return dtoarr;
        }
    }
}
