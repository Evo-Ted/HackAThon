namespace BrainHack.API.DTOs
{
    public class LikeStatusDTO
    {
        public int Count { get; set; }
        public bool UserHasLiked { get; set; }
    }

    public class AddCommentDTO
    {
        public string Content { get; set; } = string.Empty;
    }

    public class CommentResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string ArticleId { get; set; } = string.Empty;
        public string UserPseudo { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}