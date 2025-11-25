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
        private readonly Lazy<IMessageRepository> _messageRepository;
        private readonly Lazy<ICommentRepository> _commentRepository;
        private readonly Lazy<ISkillsRepository> _skillsRepository;
        private readonly Lazy<IExperinceRepository> _experinceRepository;
        private readonly Lazy<IArticleLikeRepository> _articleLikeRepository;
        public RepositoryManager(MyWebSiteData dbContext, IUserRepository userRepository, ICommentRepository commentRepository, IExperinceRepository experinceRepository, IArticleLikeRepository articleLikeRepository)
        {
            _dbContext = dbContext;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(dbContext));
            _projectRepository = new Lazy<IProjectRepository>(() => new ProjectRepository(dbContext));
            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(dbContext));
            _articleRepository = new Lazy<IArticleRepository>(() => new ArticleRepository(dbContext));
            _messageRepository = new Lazy<IMessageRepository>(() => new MessageRepository(dbContext));
            _commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(dbContext));
            _skillsRepository = new Lazy<ISkillsRepository>(() => new SkillRepository(dbContext));
            _experinceRepository = new Lazy<IExperinceRepository>(() => new ExperinceRepository(dbContext)); ;
            _articleLikeRepository = new Lazy<IArticleLikeRepository>(() => new ArticleLikeRepository(dbContext)); ;
        }

        public IUserRepository UserRepository => _userRepository.Value;

        public IProjectRepository ProjectRepository => _projectRepository.Value;

        public ICategoryRepository CategoryRepository => _categoryRepository.Value;

        public IArticleRepository ArticleRepository => _articleRepository.Value;

        public IMessageRepository MessageRepository => _messageRepository.Value;

        public ICommentRepository CommentRepository => _commentRepository.Value;

        public ISkillsRepository SkillsRepository => _skillsRepository.Value;

        public IExperinceRepository ExperinceRepository => _experinceRepository.Value;

        public IArticleLikeRepository ArticleLikeRepository => _articleLikeRepository.Value;

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
