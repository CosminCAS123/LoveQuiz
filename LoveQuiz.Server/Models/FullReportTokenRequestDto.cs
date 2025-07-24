namespace LoveQuiz.Server.Models
{
    public class FullReportTokenRequestDto
    {
        public Guid Token { get; set; }
        public List<QuizSubmissionDto> Submissions { get; set; }
    }
}
