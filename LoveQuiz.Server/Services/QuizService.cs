using LoveQuiz.Server.Models;
using System.Text.Json;
namespace LoveQuiz.Server.Services
{
    public class QuizService
    {
        private const string jsonPath = "Data/quiz_data.json";


        public async Task<IEnumerable<PublicQuestionDto>> GetQuestionsAsync(string gender)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, jsonPath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {jsonPath} not found.");

            var json = await File.ReadAllTextAsync(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

           var allQuestions = JsonSerializer.Deserialize<IEnumerable<QuestionDto>>(json, options);
            if (allQuestions == null)
                throw new JsonException("Failed to deserialize JSON data.");

           var filtered = allQuestions
                     .Where(q => q.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase))
                     .Select(q => new PublicQuestionDto
                     {
                         Id = q.Id,
                         Question = q.Question,
                         Answers = q.Answers.Select(a => new PublicAnswerDto
                          {
                            Id = a.Id,
                            Answer = a.Answer
                           }).ToList()
                      });
            var random = new Random();
            return filtered.OrderBy(_ => random.Next());


        }
    }
}
