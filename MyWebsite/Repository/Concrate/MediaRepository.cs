using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class MediaRepository : RepositoryBase<Media>, IMediaRepository
    {
        public MediaRepository(MyWebSiteData dbContext) : base(dbContext)
        {
        }
    }
}
