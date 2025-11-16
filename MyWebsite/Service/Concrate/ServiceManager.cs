using FluentValidation;
using MyWebsite.Dtos.CategoryDtos;
using MyWebsite.Dtos.MessageDtos;
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


        public ServiceManager(IRepositoryManager manager, IConfiguration configuration, IValidator<CreateCategoryDtos> categoryDto, IValidator<MessageDtos> messageDto)
        {
            _userService = new Lazy<IUserService>(() => new UserManager(manager, configuration));
            _projectService = new Lazy<IProjectService>(() => new ProjectManager(manager));
            _categoryService = new Lazy<ICategoryService>(() => new CategoryManager(manager, categoryDto));
            _articleService = new Lazy<IArticleService>(() => new ArticleManager(manager));
            _messageService = new Lazy<IMessageService>(() => new MessageManager(manager, messageDto));
            _authService = new Lazy<IAuthService>(() => new AuthService(manager, configuration));
        }

        public IUserService UserService => _userService.Value;

        public IProjectService ProjectService => _projectService.Value;

        public ICategoryService CategoryService => _categoryService.Value;

        public IArticleService ArticleService => _articleService.Value;

        public IMessageService MessageService => _messageService.Value;

        public IAuthService AuthService => _authService.Value;
    }
}
