namespace LoveQuiz.Server.Models
{
    public class PublicQuestionDto
    {
        public int Id { get; set; }
        public List<PublicAnswerDto> Answers { get; set; } = new();
        public string Question { get; set; } = string.Empty;
    }
}
