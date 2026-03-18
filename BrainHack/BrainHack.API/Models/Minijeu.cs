using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BrainHack.API.Models
{
    [Table("mini_jeux")]
    public class MiniJeu : BaseModel
    {
        [PrimaryKey("id_game", false)]
        public string IdGame { get; set; } = string.Empty;

        [Column("titre")]
        public string Titre { get; set; } = string.Empty;

        [Column("créé_à")]
        public DateTime CreeA { get; set; } = DateTime.UtcNow;
    }
}