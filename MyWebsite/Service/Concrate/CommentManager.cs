using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.Comment;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;
using System.Security.Claims;

namespace MyWebsite.Service.Concrate
{
    public class CommentManager : ICommentService
    {
        private readonly IRepositoryManager _manager;
        private readonly IValidator<createCommentDto> _createCommentValidator;
        private readonly IHttpContextAccessor _acsessor;
        public CommentManager(IRepositoryManager manager, IValidator<createCommentDto> createCommentValidator, IHttpContextAccessor acsessor)
        {
            _manager = manager;
            _createCommentValidator = createCommentValidator;
            _acsessor = acsessor;
        }

        public async Task<BaseResponse<object>> CreateComment(createCommentDto dto)
        {
            var resultValidator = await _createCommentValidator.ValidateAsync(dto);
            if (!resultValidator.IsValid)
            {
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrroCode = ErrorCodes.ValidationError,
                    Message = resultValidator.Errors.Select(er => er.ErrorMessage).FirstOrDefault()
                };

            }
            var userId = _acsessor.HttpContext?
           .User?
           .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new  UnAuthorizedException("lütfen giriş yapınız");
            }
            var comment = new Comment
            {
                Content=dto.Content,
                UserId=int.Parse(userId),
                ArticleId=dto.ArticleId

            };
            await _manager.CommentRepository.AddAsync(comment);
            await _manager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Data=null,
                Message="yorum başarılı bir şekilde eklendi",
                ErrroCode=null

            };
            
        }

        //public async Task<BaseResponse<List<ReadCommentDto>>> GetCommentById(int ArticleId)
        //{
        //    var comment = await _manager.CommentRepository.getById(ArticleId);
        //    if (comment == null)
        //        throw new NotFoundException("istenilen id ye göre yorum yok");

        //    var userName = _acsessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        //    var commentDto = new ReadCommentDto
        //    {
        //        CreatedDate = comment.CreatedDate,
        //        Id = comment.Id,
        //        Content = comment.Content,
        //        ArticleTitle = comment.Article.Title,
        //        UserName = userName
        //    };
        //    return new BaseResponse<List<ReadCommentDto>>
        //    {
        //        IsSuccess = true,
        //        Data =  commentDto,
        //        ErrroCode = null,
        //        Message = "başarılı çekilde"
        //    };
        //}

        public async Task<BaseResponse<IEnumerable<ReadCommentDto>>> gettAllComment()
        {
            var comments = await _manager.CommentRepository.FindAll().ToListAsync();
            var commentDto = comments.Select(cmnt => new ReadCommentDto
            {
                Id = cmnt.Id,
                Content = cmnt.Content,
                CreatedDate = cmnt.CreatedDate,
                UserName=cmnt.User?.Username,
                ArticleTitle = cmnt.Article?.Title

            }).ToList();

            return new BaseResponse<IEnumerable<ReadCommentDto>>
            {
                IsSuccess = true,
                Data = commentDto,
                ErrroCode = null,
                Message = "yorumlar başarılı bir şekilde çekildi"

            };
        }
    }
}
