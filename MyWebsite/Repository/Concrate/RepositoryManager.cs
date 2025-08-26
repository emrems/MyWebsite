using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly MyWebSiteData _dbContext;
        private readonly Lazy<IUserRepository>  _userRepository;

        public RepositoryManager(MyWebSiteData dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(dbContext));
        }

        public IUserRepository User => _userRepository.Value;

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
