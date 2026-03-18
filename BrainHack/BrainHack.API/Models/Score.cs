using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BrainHack.API.Models
{
    [Table("score")]
    public class Score : BaseModel
    {
        [PrimaryKey("score_id", false)]
        public string ScoreId { get; set; } = string.Empty;

        [Column("id_game")]
        public string IdGame { get; set; } = string.Empty;

        [Column("id_compte")]
        public string IdCompte { get; set; } = string.Empty;

        [Column("score_total")]
        public int ScoreTotal { get; set; }

        [Column("pourcentage_bonne_reponse")]
        public int PourcentageBonneReponse { get; set; }

        [Column("joue_le")]
        public DateTime JoueLe { get; set; } = DateTime.UtcNow;
    }
}