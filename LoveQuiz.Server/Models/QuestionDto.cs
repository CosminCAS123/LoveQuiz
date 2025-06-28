namespace LoveQuiz.Server.Models
{
    public record QuestionDto
    {

        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<AnswerDto> Answers { get; set; } = new();
    }
}
