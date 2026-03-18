using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BrainHack.API.Models
{
    [Table("quiz_question")]
    public class QuizQuestion : BaseModel
    {
        [PrimaryKey("id_quizz", false)]
        public string IdQuizz { get; set; } = string.Empty;

        [Column("question")]
        public string Question { get; set; } = string.Empty;

        [Column("réponse_vrai")]
        public string ReponseVrai { get; set; } = string.Empty;

        [Column("réponse_fausse_1")]
        public string ReponseFausse1 { get; set; } = string.Empty;

        [Column("réponse_fausse_2")]
        public string ReponseFausse2 { get; set; } = string.Empty;
    }
}