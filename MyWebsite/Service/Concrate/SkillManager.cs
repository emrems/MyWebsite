using MyWebsite.Dtos.Error;
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

        public async Task<BaseResponse<ReadSkillDtos>> GetSkillById(int id)
        {
            var skill = await _manager.SkillsRepository.GetSkillById(id);
            if(skill == null)
            {
                return new BaseResponse<ReadSkillDtos>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "yetenek bulunamadı",
                    ErrroCode = ErrorCodes.NotFound
                };
            }

            var skillDto = new ReadSkillDtos
            {
                Id = skill.Id,
                Name = skill.Name,
                Level = skill.Level
            };

            return new BaseResponse<ReadSkillDtos>
            {
                IsSuccess = true,
                Data = skillDto,
                Message = "yetenek başarılı bir şekilde çekildi",
                ErrroCode = null
            };
        }
    }
}
