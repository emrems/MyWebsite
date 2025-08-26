namespace MyWebsite.Repository.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        Task SaveAsync();
    }
}
