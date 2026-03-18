using BrainHack.API.DTOs;
using BrainHack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrainHack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _quizService.GetAll();
            return Ok(questions);
        }

        [HttpPost]
        [Authorize(Roles = "professeur")]
        public async Task<IActionResult> Create([FromBody] QuizQuestionDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Question) || string.IsNullOrEmpty(dto.ReponseVrai))
                return BadRequest(new { message = "Question et réponse vraie requises" });

            var result = await _quizService.Create(dto);
            if (result == null)
                return BadRequest(new { message = "Erreur lors de la création" });

            return Ok(result);
        }
    }
}