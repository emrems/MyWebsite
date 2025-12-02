using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Dtos.Comment;
using MyWebsite.Dtos.MediaDtos;
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
            var article = new Article
            {
                Title = articleDto.Title,
                Content = articleDto.Content,
                Slug = articleDto.Slug,
                AuthorId = articleDto.AuthorId,
                CategoryId = articleDto.CategoryId,
            };

            await _repository.ArticleRepository.AddAsync(article);

            try
            {
                // Hatanın oluştuğu yer
                await _repository.SaveAsync();

                return new BaseResponse<object>
                {
                    IsSuccess = true,
                    Message = "Article başarıyla oluşturuldu",
                    Data = null,
                    ErrroCode = null
                };
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                // 🚨 Gerçek hatayı burada yakalıyoruz.

                // 1. Konsola yazdırın
                Console.WriteLine("EF Core Hata Mesajı: " + ex.Message);

                // 2. SQL Server'dan gelen asıl iç hatayı (Inner Exception) yazdırın
                if (ex.InnerException != null)
                {
                    Console.WriteLine("SQL Server İç Hata: " + ex.InnerException.Message);
                }

                // Hata olduğunu belirten yanıtı döndürün.
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Message = "Kayıt sırasında veritabanı hatası oluştu. Logları kontrol edin.",
                    ErrroCode = "500"
                };
            }
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
            var articles = await _repository.ArticleRepository.GetAllArticlesAsync();
            if(articles == null )
            {
                throw new NotFoundException("Hiçbir article bulunamadı");
            }
            var articleDto = articles.Select(article=>new ReadArticleDtos 
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Slug = article.Slug,
                CreatedDate = article.CreatedDate,
                ArticleLikeCount = article.Likes.Count,
                IsLiked = article.IsLiked,
                AuthorName = article.Author.FullName,
                MediaFiles = article.Media.Select(m => new MediaDto
                {
                    Id = m.Id,
                    Url = m.Url,
                    Type = m.Type
                }).ToList(),
                Comments = article.Comments.Select(x => new ReadCommentDto
                {
                    ArticleTitle = x.Article.Title,
                    CreatedDate = x.CreatedDate,
                    Content = x.Content,
                    Id = x.Id,
                    UserName = x.User != null ? x.User.Username : "Anonim"
                }).ToList()
            }).AsQueryable();
            return new BaseResponse<IQueryable<ReadArticleDtos>>
            {
                IsSuccess = true,
                Message = "Başarıyla getirildi",
                Data = articleDto,
                ErrroCode = null
            };

        }

        public async Task<BaseResponse<ReadArticleDtos>> GetArticleByIdAsync(int id)
        {
            var article = await _repository.ArticleRepository.GetArticleByIdAsync(id);
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
                CreatedDate = article.CreatedDate,
                ArticleLikeCount= article.Likes.Count,
                IsLiked=article.IsLiked

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
            var authorDb = await _repository.UserRepository.GetByIdAsync(article.AuthorId);// burada jwt kullanırsam burası güncellenebilir
            var authorFulname = authorDb.FullName;

            var articleDto = new ReadArticleDtos
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Slug = article.Slug,
                CreatedDate = article.CreatedDate,
                ArticleLikeCount = article.Likes.Count,
                IsLiked = article.IsLiked,
                AuthorName = authorFulname,
                MediaFiles = article.Media.Select(m => new MediaDto
                {
                    Id = m.Id,
                    Url = m.Url,
                    Type = m.Type
                }).ToList(),
                Comments = article.Comments.Select(x => new ReadCommentDto
                {
                    ArticleTitle = x.Article.Title,
                    CreatedDate = x.CreatedDate,
                    Content = x.Content,
                    Id = x.Id,
                    UserName = x.User != null ? x.User.Username : "Anonim"
                }).ToList()
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
