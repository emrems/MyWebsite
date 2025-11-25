using MyWebsite.Dtos.ArticleLikeDtos;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;
using System.Security.Claims;

namespace MyWebsite.Service.Concrate
{
    public class ArticleLikeManager : IArticleLikeService
    {
        private readonly IRepositoryManager _manager;
        private readonly IHttpContextAccessor _contextAcsessor;

        public ArticleLikeManager(IRepositoryManager manager, IHttpContextAccessor contextAcsessor)
        {
            _manager = manager;
            _contextAcsessor = contextAcsessor;
        }

        public async Task<BaseResponse<object>> createArticleLike(CreateArticleLikeDto dto)
        {
            var userIdString = _contextAcsessor.HttpContext?.User?
                           .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var exitinLike = await _manager.ArticleLikeRepository.GetLikeUserIdAndArticleId(int.Parse(userIdString), dto.ArticleId);
            if (exitinLike)
            {
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrroCode = ErrorCodes.ValidationError,
                    Message = "Bu gönderi zaten beğenilmiş"
                };
            }
            var articleLike = new ArticleLike
            {
                ArticleId = dto.ArticleId,
                UserId = int.Parse(userIdString)
            };
            await _manager.ArticleLikeRepository.AddAsync(articleLike);
            await _manager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Data = null,
                Message = "like oluştruldu",
                ErrroCode = null
            };



        }

      
    }
}
