using BrainHack.API.DTOs;
using BrainHack.API.Models;

namespace BrainHack.API.Services
{
    public class QuizService
    {
        private readonly Supabase.Client _supabase;

        public QuizService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<QuizQuestionDTO>> GetAll()
        {
            var response = await _supabase.From<QuizQuestion>().Get();

            return response.Models.Select(q => new QuizQuestionDTO
            {
                Question = q.Question,
                ReponseVrai = q.ReponseVrai,
                ReponseFausse1 = q.ReponseFausse1,
                ReponseFausse2 = q.ReponseFausse2
            }).ToList();
        }

        public async Task<QuizQuestionDTO?> Create(QuizQuestionDTO dto)
        {
            var question = new QuizQuestion
            {
                IdQuizz = Guid.NewGuid().ToString(),
                Question = dto.Question,
                ReponseVrai = dto.ReponseVrai,
                ReponseFausse1 = dto.ReponseFausse1,
                ReponseFausse2 = dto.ReponseFausse2
            };

            var response = await _supabase.From<QuizQuestion>().Insert(question);
            var created = response.Models.FirstOrDefault();

            if (created == null) return null;

            return new QuizQuestionDTO
            {
                Question = created.Question,
                ReponseVrai = created.ReponseVrai,
                ReponseFausse1 = created.ReponseFausse1,
                ReponseFausse2 = created.ReponseFausse2
            };
        }
    }
}