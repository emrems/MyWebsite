using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;
using System.Security.Claims;

namespace MyWebsite.Service.Concrate
{
    public class ArticleManager : IArticleService
    {
        private readonly IRepositoryManager _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArticleManager(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor = null)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<object>> CreateArticleAsync(CreateArticleDtos articleDto)
        {
            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId is null)
            {
                throw new UnAuthorizedException("lütfen giriş yapınız");
            }
            var article = new Article {
                Title = articleDto.Title,
                Content = articleDto.Content,
                Slug = articleDto.Slug,
                AuthorId = articleDto.AuthorId,
                CategoryId = articleDto.CategoryId,

            };
            await _repository.ArticleRepository.AddAsync(article);
            await _repository.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Message = "Article başarıyla oluşturuldu",
                Data = null,
                ErrroCode = null
            };

        }

        public  async Task<BaseResponse<object>> DeleteArticleAsync(int id)
        {
           var article = await _repository.ArticleRepository.GetByIdAsync(id);
            if (article == null)
            {
                throw new NotFoundException("Article bulunamdı");
            }
            await _repository.ArticleRepository.DeleteAsync(article);
            await _repository.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Message = "Article başarıyla silindi",
                Data = null,
                ErrroCode = null
            };

        }

        public async Task<BaseResponse<IQueryable<ReadArticleDtos>>> GetAllArticlesAsync()
        {
            var articles = await _repository.ArticleRepository.FindAll().ToListAsync();
            if(articles == null || articles.Count == 0)
            {
                throw new NotFoundException("Hiçbir article bulunamadı");
            }
            var articleDtos = articles.Select(a => new ReadArticleDtos
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Slug = a.Slug,
                CreatedDate = a.CreatedDate
            }).AsQueryable();
            return new BaseResponse<IQueryable<ReadArticleDtos>>
            {
                IsSuccess = true,
                Message = "Başarıyla getirildi",
                Data = articleDtos,
                ErrroCode = null
            };

        }

        public async Task<BaseResponse<ReadArticleDtos>> GetArticleByIdAsync(int id)
        {
            var article = await _repository.ArticleRepository.GetByIdAsync(id);
            if (article == null)
            {
                throw new NotFoundException("Article bulunamdı");
            }
            var articleDto = new ReadArticleDtos
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Slug = article.Slug,
                CreatedDate = article.CreatedDate
            };
            return new BaseResponse<ReadArticleDtos>
            {
                IsSuccess = true,
                Message = "Başarıyla getirildi",
                Data = articleDto,
                ErrroCode = null
            };
        }

        public async Task<BaseResponse<ReadArticleDtos>> GetArticleBySlugAsync(string slug)
        {
            var article = await _repository.ArticleRepository.GetArticleBySlugAsync(slug);
            if (article == null)
            {
                throw new NotFoundException("Article bulunamdı");
            }
            var articleDto = new ReadArticleDtos
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Slug = article.Slug,
                CreatedDate = article.CreatedDate
            };
            return new BaseResponse<ReadArticleDtos>
            {
                IsSuccess = true,
                Message = "Başarıyla getirildi",
                Data = articleDto,
                ErrroCode = null
            };

        }

        public async Task<BaseResponse<object>> UpdateArticleAsync(UpdateArticleDtos articleDto)
        {
           var article = await _repository.ArticleRepository.GetByIdAsync(articleDto.Id);
            if (article == null)
            {
                throw new NotFoundException("Article bulunamadı");
            }
            article.Title = articleDto.Title;
            article.Content = articleDto.Content;
           
            await _repository.ArticleRepository.UpdateAsync(article);
            await _repository.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Message = "Article başarıyla güncellendi",
                Data = null,
                ErrroCode = null
            };

        }
    }
}
