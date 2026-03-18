namespace BrainHack.API.DTOs
{
    public class QuizQuestionDTO
    {
        public string Question { get; set; } = string.Empty;
        public string ReponseVrai { get; set; } = string.Empty;
        public string ReponseFausse1 { get; set; } = string.Empty;
        public string ReponseFausse2 { get; set; } = string.Empty;
    }
}