using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class ServiceManager : IServiceManager

    {
        private readonly Lazy<IUserService> _userService;

        public ServiceManager(IRepositoryManager manager)
        {
            _userService = new Lazy<IUserService>(() => new UserManager(manager));
        }

        public IUserService User => _userService.Value;
    }
}
