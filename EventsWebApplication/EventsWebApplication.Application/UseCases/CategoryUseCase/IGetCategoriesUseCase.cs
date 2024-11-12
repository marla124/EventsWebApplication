using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.CategoryUseCase
{
    public interface IGetCategoriesUseCase
    {
        public Task<CategoryDto[]?> Execute(CancellationToken cancellationToken);
    }
}
