using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;

namespace EventsWebApplication.BL
{
    public class CategoryService : Service<CategoryDto, Category>, ICategoryService
    {
        public CategoryService(IRepository<Category> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
