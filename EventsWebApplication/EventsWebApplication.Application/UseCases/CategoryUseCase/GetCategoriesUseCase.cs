using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.CategoryUseCase
{
    public class GetCategoriesUseCase(IMapper mapper, IUnitOfWork unitOfWork) : IGetCategoriesUseCase
    {
        public async Task<CategoryDto[]?> Execute(CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.CategoryRepository.GetCategories(cancellationToken);

            var dtoArr = categories.Select(category => mapper.Map<CategoryDto>(category)).ToArray();
            return dtoArr;
        }
    }
}
