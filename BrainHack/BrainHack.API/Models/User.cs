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

        [Column("e-mail")]
        public string Email { get; set; } = string.Empty;

        [Column("rôle")]
        public string Role { get; set; } = "eleve";

        [Column("URL de l'avatar")]
        public string AvatarUrl { get; set; } = string.Empty;
    }
}