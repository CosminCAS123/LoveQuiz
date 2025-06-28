namespace LoveQuiz.Server.Models
{
    public class AnswerDto
    {
        public string Id { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public int Points { get; set; }
    }
}
