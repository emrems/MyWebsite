using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.CategoryDtos;
using MyWebsite.Dtos.Error;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : BaseController
    {
        private readonly IServiceManager _serviceManager;
        public CategoryController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDtos categoryDto)
        {
            var result =await _serviceManager.CategoryService.AddCategory(categoryDto);
            return CreateResponse(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _serviceManager.CategoryService.GetAllCategories();
            return CreateResponse(categories);
        }

        [HttpGet("getCategoriesById")]
        public async Task<IActionResult> GetCategoriesById(int id)
        {
            var categories = await _serviceManager.CategoryService.GetCategoryById(id);
            return CreateResponse(categories);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
             var result =await _serviceManager.CategoryService.DeleteCategory(id);
            return CreateResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDtos categoryDto)
        {
            var result =await _serviceManager.CategoryService.UpdateCategory(categoryDto);
            return CreateResponse(result);
        }
    }
}
