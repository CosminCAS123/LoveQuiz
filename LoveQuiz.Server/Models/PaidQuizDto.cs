using System.Text.Json;

namespace LoveQuiz.Server.Models
{
    public class PaidQuizDto
    {
        public string Email { get; set; }
        public string? Gender { get; set; }
        public List<QuizSubmissionDto> Answers{ get; set; }

        public string? PhoneNumber { get; set; }

        public PaidQuiz ToEntity()
        {
            return new PaidQuiz
            {
                Email = this.Email,
                Gender = this.Gender,
                PhoneNumber = this.PhoneNumber,
                AnswersJson = JsonSerializer.Serialize(this.Answers),
                CreatedAt = DateTime.UtcNow
            };
        }

    }
}
