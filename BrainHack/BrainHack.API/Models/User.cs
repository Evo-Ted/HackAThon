using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BrainHack.API.Models
{
    [Table("compte")]
    public class User : BaseModel
    {
        [PrimaryKey("id_compte", false)]
        public string IdCompte { get; set; } = string.Empty;

        [Column("pseudo")]
        public string Pseudo { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("role")]
        public string Role { get; set; } = "Etudiant";

        [Column("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;
    }
}