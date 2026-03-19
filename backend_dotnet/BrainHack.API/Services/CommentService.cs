using BrainHack.API.DTOs;
using BrainHack.API.Models;

namespace BrainHack.API.Services
{
    public class CommentService
    {
        private readonly Supabase.Client _supabase;

        public CommentService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<CommentResponseDTO>> GetComments(string articleId)
        {
            var response = await _supabase
                .From<ArticleComment>()
                .Where(c => c.ArticleId == articleId)
                .Order("created_at", Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();

            return response.Models.Select(c => new CommentResponseDTO
            {
                Id = c.Id,
                ArticleId = c.ArticleId,
                UserPseudo = c.UserPseudo,
                Content = c.Content,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public async Task<CommentResponseDTO?> AddComment(string articleId, string userId, string userPseudo, string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return null;

            var comment = new ArticleComment
            {
                Id = Guid.NewGuid().ToString(),
                ArticleId = articleId,
                UserId = userId,
                UserPseudo = userPseudo,
                Content = content.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            var response = await _supabase.From<ArticleComment>().Insert(comment);
            var created = response.Models.FirstOrDefault();
            if (created == null) return null;

            return new CommentResponseDTO
            {
                Id = created.Id,
                ArticleId = created.ArticleId,
                UserPseudo = created.UserPseudo,
                Content = created.Content,
                CreatedAt = created.CreatedAt
            };
        }

        public async Task<int> GetCommentCount(string articleId)
        {
            var response = await _supabase
                .From<ArticleComment>()
                .Where(c => c.ArticleId == articleId)
                .Get();

            return response.Models.Count;
        }
    }
}
