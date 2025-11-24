using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class ExperinceRepository : RepositoryBase<Experience>, IExperinceRepository
    {
        public ExperinceRepository(MyWebSiteData dbContext) : base(dbContext)
        {
            
        }
    }
}
