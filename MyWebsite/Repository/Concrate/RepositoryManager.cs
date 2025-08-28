using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly MyWebSiteData _dbContext;
        private readonly Lazy<IUserRepository>  _userRepository;
        private readonly Lazy<IProjectRepository> _projectRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<IArticleRepository> _articleRepository;

        public RepositoryManager(MyWebSiteData dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(dbContext));
            _projectRepository = new Lazy<IProjectRepository>(() => new ProjectRepository(dbContext));
            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(dbContext));
            _articleRepository = new Lazy<IArticleRepository>(() => new ArticleRepository(dbContext));
        }

        public IUserRepository UserRepository => _userRepository.Value;

        public IProjectRepository ProjectRepository => _projectRepository.Value;

        public ICategoryRepository CategoryRepository => _categoryRepository.Value;

        public IArticleRepository ArticleRepository => _articleRepository.Value;

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
