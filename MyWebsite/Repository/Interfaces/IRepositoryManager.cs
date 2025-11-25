namespace MyWebsite.Repository.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IArticleRepository ArticleRepository { get; }
        IMessageRepository MessageRepository { get; }
        ICommentRepository CommentRepository { get; }
        ISkillsRepository SkillsRepository { get; }
        IExperinceRepository ExperinceRepository { get; }
        IArticleLikeRepository ArticleLikeRepository { get; }
        Task SaveAsync();
    }
}
