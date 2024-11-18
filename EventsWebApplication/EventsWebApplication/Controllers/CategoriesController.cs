using AutoMapper;
using EventsWebApplication.Application.UseCases.CategoryUseCase;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoriesController(IGetCategoriesUseCase getCategoriesUseCase, IMapper mapper) : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategory(CancellationToken cancellationToken)
        {
            var category = await getCategoriesUseCase.Execute(cancellationToken);
            var categoryMap = mapper.Map<List<CategoryModel>>(category);
            return Ok(categoryMap);
        }
    }
}
