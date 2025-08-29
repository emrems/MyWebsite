namespace MyWebsite.Repository.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IArticleRepository ArticleRepository { get; }
        IMessageRepository MessageRepository { get; }
        Task SaveAsync();
    }
}
