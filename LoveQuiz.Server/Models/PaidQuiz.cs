namespace LoveQuiz.Server.Models
{
    public class PaidQuiz
    {

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? Gender { get; set; }
        public string? AnswersJson { get; set; }

        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }

\
    }
}
