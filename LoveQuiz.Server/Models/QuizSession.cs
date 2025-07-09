namespace LoveQuiz.Server.Models
{
    public class QuizSession
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string AnswersJson { get; set; }
        public string StripeRef { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
