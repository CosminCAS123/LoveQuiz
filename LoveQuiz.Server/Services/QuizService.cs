using LoveQuiz.Server.Models;
using System.Text.Json;

namespace LoveQuiz.Server.Services
{
    public class QuizService
    {
        private const string jsonPath = "Data/quiz_data.json";


        public async Task<IEnumerable<QuestionDto>> GetQuestionsAsync()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, jsonPath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {jsonPath} not found.");

            var json = await File.ReadAllTextAsync(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var questions = JsonSerializer.Deserialize<IEnumerable<QuestionDto>>(json, options);

            return questions ?? Enumerable.Empty<QuestionDto>();
        }
    }
}
