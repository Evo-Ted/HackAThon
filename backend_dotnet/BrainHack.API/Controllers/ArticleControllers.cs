using BrainHack.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrainHack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService _articleService;

        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { message = "id requis" });

            var article = await _articleService.GetByIdAsync(id);

            if (article == null)
                return NotFound(new { message = "Article introuvable" });

            return Ok(new
            {
                id = article.Id,
                title = article.Title,
                intro = article.Intro,
                sections = article.Sections
            });
        }
    }
}