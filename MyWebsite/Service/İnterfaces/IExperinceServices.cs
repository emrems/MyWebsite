using MyWebsite.Dtos.Experince;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Service.İnterfaces
{
    public interface IExperinceServices
    {
        Task<BaseResponse<IEnumerable<ReadExperinceDto>>> GetAllExperinceAsync();
        Task<BaseResponse<ReadExperinceDto>> GetExperinceById(int id);
        Task<BaseResponse<object>> createExperienceAsync(CreateExperinceDtos dto);
    }
}
