namespace LoveQuiz.Server.Models
{
    public class AnswerDto
    {
           public int Id { get; set; }
        public string Answer { get; set; } = string.Empty;
        public int Points { get; set; }
        public string? Flag { get; set; } // ex: "gaslight", "avoidance", etc. (opțional)
    }

}
