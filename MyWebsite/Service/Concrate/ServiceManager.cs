using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class ServiceManager : IServiceManager

    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IProjectService> _projectService;
        private readonly Lazy<ICategoryService> _categoryService;

        public ServiceManager(IRepositoryManager manager)
        {
            _userService = new Lazy<IUserService>(() => new UserManager(manager));
            _projectService = new Lazy<IProjectService>(() => new ProjectManager(manager));
            _categoryService = new Lazy<ICategoryService>(() => new CategoryManager(manager));
        }

        public IUserService UserService => _userService.Value;

        public IProjectService ProjectService => _projectService.Value;

        public ICategoryService CategoryService => _categoryService.Value;
    }
}
