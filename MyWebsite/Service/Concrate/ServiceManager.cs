using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IProjectService> _projectService;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IArticleService> _articleService;
        private readonly Lazy<IMessageService> _messageService;
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<ICommentService> _commentService;
        private readonly IServiceProvider _serviceProvider;

        public ServiceManager(IRepositoryManager manager, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _userService = new Lazy<IUserService>(() =>
                ActivatorUtilities.CreateInstance<UserManager>(_serviceProvider, manager, configuration));

            _projectService = new Lazy<IProjectService>(() =>
                ActivatorUtilities.CreateInstance<ProjectManager>(_serviceProvider, manager));

            _categoryService = new Lazy<ICategoryService>(() =>
                ActivatorUtilities.CreateInstance<CategoryManager>(_serviceProvider, manager));

            _articleService = new Lazy<IArticleService>(() =>
                ActivatorUtilities.CreateInstance<ArticleManager>(_serviceProvider, manager));

            _messageService = new Lazy<IMessageService>(() =>
                ActivatorUtilities.CreateInstance<MessageManager>(_serviceProvider, manager));

            _authService = new Lazy<IAuthService>(() =>
                ActivatorUtilities.CreateInstance<AuthService>(_serviceProvider, manager, configuration));
            _commentService = new Lazy<ICommentService>(() =>
                ActivatorUtilities.CreateInstance<CommentManager>(_serviceProvider, manager));
        }

        public IUserService UserService => _userService.Value;
        public IProjectService ProjectService => _projectService.Value;
        public ICategoryService CategoryService => _categoryService.Value;
        public IArticleService ArticleService => _articleService.Value;
        public IMessageService MessageService => _messageService.Value;
        public IAuthService AuthService => _authService.Value;

        public ICommentService CommentService => _commentService.Value;
    }
}
