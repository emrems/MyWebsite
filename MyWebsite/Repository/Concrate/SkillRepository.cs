using Microsoft.EntityFrameworkCore;
using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class SkillRepository : RepositoryBase<Skill>, ISkillsRepository
    {
        public SkillRepository(MyWebSiteData dbContext) : base(dbContext)
        {
        }

        public  IQueryable<Skill> GetAllSkills()
        {
            var skills =  FindAll();
            return skills;
            
        }
    }
}
