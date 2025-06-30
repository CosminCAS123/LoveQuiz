namespace LoveQuiz.Server.Models
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Gender { get; set; } = "unisex"; // "male", "female" sau "unisex"
        public string Question { get; set; } = string.Empty;
        public List<AnswerDto> Answers { get; set; } = new();
    }

}
