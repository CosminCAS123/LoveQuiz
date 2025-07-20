using System.Text.Json;

namespace LoveQuiz.Server.Models
{
    public class PaidQuizDto
    {
        public string Email { get; set; }
        public string? Gender { get; set; }
        public List<QuizSubmissionDto> Answers{ get; set; }


        public PaidQuiz ToEntity()
        {
            return new PaidQuiz
            {
                Email = this.Email,
                Gender = this.Gender,
                AnswersJson = JsonSerializer.Serialize(this.Answers),
                CreatedAt = DateTime.UtcNow
            };
        }

    }
}
