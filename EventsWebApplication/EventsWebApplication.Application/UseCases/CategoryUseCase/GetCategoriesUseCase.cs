using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.GeneralUseCases;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.CategoryUseCase
{
    public class GetCategoriesUseCase : GetManyUseCase<CategoryDto, Category>, IGetCategoriesUseCase
    {
        public GetCategoriesUseCase(IRepository<Category> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
