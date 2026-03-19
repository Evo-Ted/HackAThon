using BrainHack.API.DTOs;
using BrainHack.API.Models;

namespace BrainHack.API.Services
{
    public class LikeService
    {
        private readonly Supabase.Client _supabase;

        public LikeService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<LikeStatusDTO> GetLikeStatus(string articleId, string userId)
        {
            var response = await _supabase
                .From<ArticleLike>()
                .Where(l => l.ArticleId == articleId)
                .Get();

            var likes = response.Models;
            return new LikeStatusDTO
            {
                Count = likes.Count,
                UserHasLiked = likes.Any(l => l.UserId == userId)
            };
        }

        public async Task<LikeStatusDTO> ToggleLike(string articleId, string userId)
        {
            var existing = await _supabase
                .From<ArticleLike>()
                .Where(l => l.ArticleId == articleId && l.UserId == userId)
                .Get();

            if (existing.Models.Any())
            {
                // Déjà liké → on retire
                await _supabase
                    .From<ArticleLike>()
                    .Where(l => l.ArticleId == articleId && l.UserId == userId)
                    .Delete();
            }
            else
            {
                // Pas encore liké → on ajoute
                var like = new ArticleLike
                {
                    Id = Guid.NewGuid().ToString(),
                    ArticleId = articleId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                await _supabase.From<ArticleLike>().Insert(like);
            }

            return await GetLikeStatus(articleId, userId);
        }
    }
}