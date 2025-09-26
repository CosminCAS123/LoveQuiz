namespace LoveQuiz.Server.Models
{
    public class FullReportSessionRequestDto
    {
        public Guid SessionId { get; set; }
        public List<QuizSubmissionDto> Submissions { get; set; }
    }
}
