using BrainHack.API.DTOs;
using BrainHack.API.Models;

namespace BrainHack.API.Services
{
    public class ScoreService
    {
        private readonly Supabase.Client _supabase;

        public ScoreService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<ScoreResponseDTO?> SaveScore(string idCompte, ScoreDTO dto)
        {
            var score = new Score
            {
                ScoreId = Guid.NewGuid().ToString(),
                IdGame = dto.IdGame,
                IdCompte = idCompte,
                ScoreTotal = dto.ScoreTotal,
                PourcentageBonneReponse = dto.PourcentageBonneReponse,
                JoueLe = DateTime.UtcNow
            };

            var response = await _supabase.From<Score>().Insert(score);
            var saved = response.Models.FirstOrDefault();

            if (saved == null) return null;

            return new ScoreResponseDTO
            {
                IdGame = saved.IdGame,
                ScoreTotal = saved.ScoreTotal,
                PourcentageBonneReponse = saved.PourcentageBonneReponse,
                JoueLe = saved.JoueLe
            };
        }

        public async Task<List<ScoreResponseDTO>> GetUserScores(string idCompte)
        {
            var response = await _supabase
                .From<Score>()
                .Where(s => s.IdCompte == idCompte)
                .Get();

            return response.Models.Select(s => new ScoreResponseDTO
            {
                IdGame = s.IdGame,
                ScoreTotal = s.ScoreTotal,
                PourcentageBonneReponse = s.PourcentageBonneReponse,
                JoueLe = s.JoueLe
            }).ToList();
        }
    }
}