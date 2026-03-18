namespace BrainHack.API.DTOs
{
    public class ScoreDTO
    {
        public string IdGame { get; set; } = string.Empty;
        public int ScoreTotal { get; set; }
        public int PourcentageBonneReponse { get; set; }
    }

    public class ScoreResponseDTO
    {
        public string IdGame { get; set; } = string.Empty;
        public int ScoreTotal { get; set; }
        public int PourcentageBonneReponse { get; set; }
        public DateTime JoueLe { get; set; }
    }
}