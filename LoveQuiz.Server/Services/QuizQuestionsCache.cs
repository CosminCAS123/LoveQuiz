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

        public List<QuizSubmissionTextDto> GetSelectedTextPairs(List<QuizSubmissionDto> submissions)
        {
            var result = new List<QuizSubmissionTextDto>();

            foreach (var submission in submissions)
            {
                var question = this.Questions.FirstOrDefault(q => q.Id == submission.QuestionId);
                if (question == null)
                    throw new KeyNotFoundException($"Question with ID {submission.QuestionId} not found in cache.");

                var answer = question.Answers.FirstOrDefault(a => a.Id == submission.AnswerId);
                if (answer == null) 
                    throw new KeyNotFoundException($"Answer with ID {submission.AnswerId} not found in question ID {submission.QuestionId}.");
                ;

                result.Add(new QuizSubmissionTextDto
                {
                    QuestionText = question.Question,
                    AnswerText = answer.Answer
                });
            }

            return result;
        }


    }
}
