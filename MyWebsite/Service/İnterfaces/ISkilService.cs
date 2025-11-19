using MyWebsite.Dtos.Response;
using MyWebsite.Dtos.SkillDtos;

namespace MyWebsite.Service.İnterfaces
{
    public interface ISkilService
    {
        Task<BaseResponse<IEnumerable<ReadSkillDtos>>> GetAllSkills();
     }
}
