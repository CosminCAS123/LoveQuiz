namespace LoveQuiz.Server.Models
{
    public class QuizVisitDto
    {
        public string Email { get; set; }
        public string? Gender { get; set; }

        public QuizVisit ToEntity()
        {
            return new QuizVisit
            {
                Email = this.Email,
                Gender = this.Gender,
                Converted = false,
                CreatedAt = DateTime.UtcNow
            };
        }
    }

}
