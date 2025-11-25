using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/article")]
    public class ArticleController : BaseController
    {
        private readonly IServiceManager _manager;

        public ArticleController( IServiceManager manager)
        {
            _manager = manager;
        }
       // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {

            var article = await _manager.ArticleService.GetArticleByIdAsync(id);

            return CreateResponse(article);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _manager.ArticleService.GetAllArticlesAsync();
            return CreateResponse(articles);
        }

       // [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleDtos articleDto)
        {
            var result =await _manager.ArticleService.CreateArticleAsync(articleDto);
            return CreateResponse(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] UpdateArticleDtos articleDto)
        {
             var result =await _manager.ArticleService.UpdateArticleAsync(articleDto);
            return CreateResponse(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var result= await _manager.ArticleService.DeleteArticleAsync(id);
            return CreateResponse(result);
        }

      //  [Authorize]
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetArticleBySlug(string slug)
        {
            var result = await _manager.ArticleService.GetArticleBySlugAsync(slug);
            return CreateResponse(result);
        }
       
    }
}
