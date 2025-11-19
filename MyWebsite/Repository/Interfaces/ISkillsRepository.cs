using MyWebsite.Entities;

namespace MyWebsite.Repository.Interfaces
{
    public interface ISkillsRepository : IRepositoryBase<Skill>
    {
        IQueryable<Skill> GetAllSkills();

    }
}
