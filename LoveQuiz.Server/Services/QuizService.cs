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
        public QuizService(IWebHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, jsonPath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {jsonPath} not found.");

            var json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            try
            {
                all_questions = JsonSerializer.Deserialize<IEnumerable<QuestionDto>>(json, options);
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

            return filtered.OrderBy(_ => Random.Shared.Next()).ToList();

        }

        public  NoPaymentReport GetFreeReport(IEnumerable<QuizSubmissionDto> submissions)
        {
            var flagCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var s in submissions)
            {
                var question = all_questions.FirstOrDefault(q => q.Id == s.QuestionId);
                var answer = question?.Answers.FirstOrDefault(a => a.Id == s.AnswerId);
                if (!string.IsNullOrEmpty(answer?.Flag))
                {
                    flagCounts[answer.Flag] = flagCounts.TryGetValue(answer.Flag, out var c) ? c + 1 : 1;
                }
            }
                // ---------- 2.  No red-flags case ----------
                if (flagCounts.Count == 0)
                    return new NoPaymentReport
                    {
                        Title = "Nicio alarmă majoră deocamdată",
                        Teaser = "Totuși, ar putea exista nuanțe ascunse. Vrei să afli mai multe?",
                        SeverityLevel = 0
                    };

                // ---------- 3.  Severity map ----------
                var severityMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                {
                    ["insecurity"] = 1,
                    ["avoidance"] = 1,
                    ["dependency"] = 2,
                    ["manipulation"] = 3,
                    ["control"] = 3
                };

                // ---------- 4. Find worst flag ----------

                var worstFlag = flagCounts.OrderByDescending(f => f.Value)
                    .ThenByDescending(f => severityMap.GetValueOrDefault(f.Key, 0))
                    .First().Key;

                var level = severityMap.GetValueOrDefault(worstFlag, 0);

                return new NoPaymentReport
                {
                    Title = $"Semne de **{worstFlag}**",
                    Teaser = "Acesta e doar vârful aisbergului. Descoperă întreaga analiză pentru a afla impactul real.",
                    SeverityLevel = level
                };

            }
        }


    }

