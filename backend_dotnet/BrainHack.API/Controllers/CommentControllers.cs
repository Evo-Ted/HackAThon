using BrainHack.API.DTOs;
using BrainHack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainHack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{articleId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(string articleId)
        {
            var comments = await _commentService.GetComments(articleId);
            return Ok(comments);
        }

        [HttpPost("{articleId}")]
        public async Task<IActionResult> AddComment(string articleId, [FromBody] AddCommentDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest(new { message = "Le commentaire ne peut pas être vide." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userPseudo = User.FindFirstValue("pseudo");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var comment = await _commentService.AddComment(articleId, userId, userPseudo ?? "Anonyme", dto.Content);
            if (comment == null)
                return BadRequest(new { message = "Impossible d'ajouter le commentaire." });

            return Ok(comment);
        }

        [HttpGet("{articleId}/count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentCount(string articleId)
        {
            var count = await _commentService.GetCommentCount(articleId);
            return Ok(new { count });
        }
    }
}