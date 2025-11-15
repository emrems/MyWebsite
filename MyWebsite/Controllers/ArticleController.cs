using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Exceptions;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/article")]
    public class ArticleController : ControllerBase
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

            return Ok(article);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _manager.ArticleService.GetAllArticlesAsync();
            return Ok(articles);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleDtos articleDto)
        {
            await _manager.ArticleService.CreateArticleAsync(articleDto);
           return Ok("Article başarılı bir şekilde oluştruldu");
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] UpdateArticleDtos articleDto)
        {
             await _manager.ArticleService.UpdateArticleAsync(articleDto);
            return Ok("başarılı bir şekilde güncellendi");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
           await _manager.ArticleService.DeleteArticleAsync(id);
            return NoContent();
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetArticleBySlug(string slug)
        {
            var article = await _manager.ArticleService.GetArticleBySlugAsync(slug);
            return Ok(article);
        }
       
    }
}
