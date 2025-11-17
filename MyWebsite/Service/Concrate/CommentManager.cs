using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.Comment;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class CommentManager : ICommentService
    {
        private readonly IRepositoryManager _manager;
        private readonly IValidator<createCommentDto> _createCommentValidator;
        public CommentManager(IRepositoryManager manager, IValidator<createCommentDto> createCommentValidator)
        {
            _manager = manager;
            _createCommentValidator = createCommentValidator;
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
            var comment = new Comment
            {
                Content=dto.Content,
                UserId=dto.UserId,
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

        public async Task<BaseResponse<IEnumerable<ReadCommentDto>>> gettAllComment()
        {
            var comments = await _manager.CommentRepository.FindAll().ToListAsync();
            var commentDto = comments.Select(cmnt => new ReadCommentDto
            {
                Id = cmnt.Id,
                Content = cmnt.Content,
                CreatedDate = cmnt.CreatedDate,
                User = cmnt.User,
                UserId=cmnt.UserId,
                ArticleId = cmnt.ArticleId,
                Article = cmnt.Article

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
