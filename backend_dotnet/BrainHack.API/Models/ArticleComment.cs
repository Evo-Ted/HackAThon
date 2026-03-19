using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BrainHack.API.Models
{
    [Table("article_comments")]
    public class ArticleComment : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = string.Empty;

        [Column("article_id")]
        public string ArticleId { get; set; } = string.Empty;

        [Column("user_id")]
        public string UserId { get; set; } = string.Empty;

        [Column("user_pseudo")]
        public string UserPseudo { get; set; } = string.Empty;

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}