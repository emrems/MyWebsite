using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(MyWebSiteData dbContext) : base(dbContext)
        {
        }
    }
}
