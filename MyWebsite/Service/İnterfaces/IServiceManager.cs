namespace MyWebsite.Service.İnterfaces
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        IProjectService ProjectService { get; }
        ICategoryService CategoryService { get; }
        IArticleService ArticleService { get; }
        IMessageService MessageService { get; }
        IAuthService AuthService { get; }
        ICommentService CommentService { get; }


    }
}
