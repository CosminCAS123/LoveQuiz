using LoveQuiz.Server.Models;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json;
namespace LoveQuiz.Server.Services
{
    public class QuizService
    {
        private const string jsonPath = "Data/quiz_data.json";
        private readonly IEnumerable<QuestionDto>all_questions;
        private readonly IEnumerable<PublicQuestionDto> public_questions;

        public QuizService(IWebHostEnvironment env)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, jsonPath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {jsonPath} not found.");

            var json = File.ReadAllTextAsync(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            try
            {
                all_questions = JsonSerializer.Deserialize<IEnumerable<QuestionDto>>(json.Result, options);
            }
            catch (JsonException ex)
            {
                throw new JsonException("Failed to deserialize JSON data.", ex);
            }
           
        }
        
        public  IEnumerable<PublicQuestionDto> GetAllQuestions(string gender)
        {

            var filtered = all_questions
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

            Random rnd = new Random();
            return filtered
                           .OrderBy(q => rnd.Next())
                           .ToList();




        }
    }
}
