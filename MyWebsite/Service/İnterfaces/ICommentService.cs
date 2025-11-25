using MyWebsite.Dtos.Comment;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Service.İnterfaces
{
    public interface ICommentService
    {
        Task<BaseResponse<IEnumerable<ReadCommentDto>>> gettAllComment();
        Task<BaseResponse<object>> CreateComment(createCommentDto dto);
        //Task<BaseResponse<List<ReadCommentDto>>> GetCommentById(int ArticleId);
    }
}
