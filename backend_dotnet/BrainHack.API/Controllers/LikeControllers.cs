using BrainHack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainHack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly LikeService _likeService;

        public LikeController(LikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetLikeStatus(string articleId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var status = await _likeService.GetLikeStatus(articleId, userId);
            return Ok(status);
        }

        [HttpPost("{articleId}")]
        public async Task<IActionResult> ToggleLike(string articleId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var status = await _likeService.ToggleLike(articleId, userId);
            return Ok(status);
        }
    }
}