using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class ArticleManager : IArticleService
    {
        private readonly IRepositoryManager _repository;

        public ArticleManager(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task CreateArticleAsync(CreateArticleDtos articleDto)
        {
            var article = new Article {
                Title = articleDto.Title,
                Content = articleDto.Content,
                Slug = articleDto.Slug,
                AuthorId = articleDto.AuthorId,
                CategoryId = articleDto.CategoryId,

            };
            await _repository.ArticleRepository.AddAsync(article);
            await _repository.SaveAsync();

        }

        public  async Task DeleteArticleAsync(int id)
        {
           var article = await _repository.ArticleRepository.GetByIdAsync(id);
            if (article == null)
            {
                throw new NotFoundException("Article bulunamdı");
            }
            await _repository.ArticleRepository.DeleteAsync(article);
            await _repository.SaveAsync();

        }

        public async Task<IQueryable<ReadArticleDtos>> GetAllArticlesAsync()
        {
            var articles = await _repository.ArticleRepository.FindAll().ToListAsync();
            var articleDtos = articles.Select(a => new ReadArticleDtos
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Slug = a.Slug,
                CreatedDate = a.CreatedDate
            }).AsQueryable();
            return articleDtos;

        }

        public async Task<ReadArticleDtos> GetArticleByIdAsync(int id)
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
            return articleDto;
        }

        public async Task UpdateArticleAsync(UpdateArticleDtos articleDto)
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
           
        }
    }
}
