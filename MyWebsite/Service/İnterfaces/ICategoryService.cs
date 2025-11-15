using MyWebsite.Dtos.CategoryDtos;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Service.İnterfaces
{
    public interface ICategoryService
    {
        Task<BaseResponse<IEnumerable<ReadCategoryDtos>>> GetAllCategories();
        Task<BaseResponse<object>> AddCategory(CreateCategoryDtos category);
        Task<BaseResponse<object>> DeleteCategory(int id);

        Task<BaseResponse<object>> UpdateCategory(UpdateCategoryDtos dto);
        Task<BaseResponse<ReadCategoryDtos>> GetCategoryById(int id);

    }
}
