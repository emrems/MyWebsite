using MyWebsite.Dtos.CategoryDtos;

namespace MyWebsite.Service.İnterfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<ReadCategoryDtos>> GetAllCategories();
        Task AddCategory(CreateCategoryDtos category);
        Task DeleteCategory(int id);
       
        Task UpdateCategory(UpdateCategoryDtos dto);

    }
}
