using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.CategoryDtos;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public CategoryController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDtos categoryDto)
        {
            await _serviceManager.CategoryService.AddCategory(categoryDto);
            return Ok("category başarılı bir şekilde eklendi");

        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _serviceManager.CategoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _serviceManager.CategoryService.DeleteCategory(id);
            return Ok("category başarılı bir şekilde silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDtos categoryDto)
        {
            await _serviceManager.CategoryService.UpdateCategory(categoryDto);
            return Ok("category başarılı bir şekilde güncellendi");
        }
    }
}
