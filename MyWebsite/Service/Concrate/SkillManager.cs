using MyWebsite.Dtos.Response;
using MyWebsite.Dtos.SkillDtos;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class SkillManager : ISkilService
    {
        private readonly IRepositoryManager _manager;

        public SkillManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<BaseResponse<IEnumerable<ReadSkillDtos>>> GetAllSkills()
        {
            var skills = _manager.SkillsRepository.GetAllSkills();
            var skillDto = skills.Select(skill => new ReadSkillDtos
            {
                Id = skill.Id,
                Name = skill.Name,
                Level = skill.Level
            });
            return new BaseResponse<IEnumerable<ReadSkillDtos>>
            {
                IsSuccess = true,
                Data = skillDto,
                Message = "Yetenekler başarılı bir şekilde çekildi",
                ErrroCode = null
            };
        }
    }
}
