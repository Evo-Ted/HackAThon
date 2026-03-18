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
    public class ScoreController : ControllerBase
    {
        private readonly ScoreService _scoreService;

        public ScoreController(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveScore([FromBody] ScoreDTO dto)
        {
            var idCompte = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _scoreService.SaveScore(idCompte, dto);

            if (result == null)
                return BadRequest(new { message = "Erreur lors de la sauvegarde du score" });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyScores()
        {
            var idCompte = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var scores = await _scoreService.GetUserScores(idCompte);
            return Ok(scores);
        }
    }
}