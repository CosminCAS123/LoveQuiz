using LoveQuiz.Server.Models;
using System.Text.Json;

namespace LoveQuiz.Server.Services
{
    public class QuizQuestionsCache
    {
        public IReadOnlyList<QuestionDto> Questions { get; }

        public QuizQuestionsCache(IWebHostEnvironment env)
        {
            var json = File.ReadAllText(Path.Combine(env.ContentRootPath, "Data", "quiz_data.json"));
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Questions = JsonSerializer.Deserialize<List<QuestionDto>>(json , options)!;
        }


    }
}
