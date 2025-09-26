namespace LoveQuiz.Server.Models
{
    public class QuizSession
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? Gender { get; set; }
        public bool Converted { get; set; } = false;
        public DateTime CreatedAt { get; set; }

    }


}
