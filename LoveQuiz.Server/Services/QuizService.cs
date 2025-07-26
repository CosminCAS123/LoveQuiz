using LoveQuiz.Server.Models;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Reflection;
using System.Text.Json;
namespace LoveQuiz.Server.Services
{
    public class QuizService
    {
        private const string jsonPath = "Data/quiz_data.json";
        private const string freeTeaser = "Acesta e doar vârful aisbergului. Descoperă întreaga analiză pentru a afla impactul real.";
        private const string edgeFreeTeaser = "Totuși, există nuanțe ascunse. Vrei să afli mai multe?";
        private readonly QuizSessionRepository quiz_repo;
        private readonly OpenAIReportService openAI_service;
        private readonly QuizQuestionsCache questions_cache;
        public QuizService(QuizQuestionsCache cache, OpenAIReportService openAI, QuizSessionRepository quiz_repo)
        {
            this.questions_cache = cache;
            this.quiz_repo = quiz_repo;
            this.openAI_service = openAI;
        }
        public async Task<FinalReport> GetFullReportAsync(List<QuizSubmissionDto> submissions)
        {
            var textPairs = questions_cache.GetSelectedTextPairs(submissions);

            var prompt = PromptBuilder.BuildPrompt(textPairs);

            return await openAI_service.GetReportOpenAIAsync(prompt);
        }

        public IEnumerable<PublicQuestionDto> GetAllQuestions(string gender)
        {

            var filtered = questions_cache.Questions
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

        public NoPaymentReport GetFreeReport(IEnumerable<QuizSubmissionDto> submissions)
        {
            var flagCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var s in submissions)
            {
                var question = questions_cache.Questions.FirstOrDefault(q => q.Id == s.QuestionId);
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
                    Teaser = edgeFreeTeaser,
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
            var count = flagCounts[worstFlag];


            if (level == 1 && count >= 3) level = 2; // If insecurity or avoidance is reported 3+ times, treat it as level 2
            if (level == 2 && count >= 4) level = 3; // If dependency is reported 4+ times, treat it as level 3


            worstFlag = worstFlag switch
            {
                "insecurity" => "insecuritate",
                "avoidance" => "evitare (emoțională)",
                "dependency" => "dependență emoțională",
                "manipulation" => "manipulare",
                "control" => "control emoțional",
                _ => throw new KeyNotFoundException("Didn't get the worst flag correctly")
            };

            return new NoPaymentReport
            {
                Title = $"Semne de **{worstFlag}**",
                Teaser = freeTeaser,
                SeverityLevel = level //EMOJI/COLOR PALLETE BASED ON LEVEL
                                      // SeverityLevel: 0 = no flags, 1 = minor, 2 = moderate, 3 = severe

            };

        }
        public async Task AddFreeQuizSessionAsync(QuizSessionDto dto)
        {
            

            var session = new QuizSession
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                Gender = dto.Gender,
                Converted = false,
                CreatedAt = DateTime.UtcNow,
                TokenUsed = false,
                AccessToken = null
                // Later you can add: PaymentStatus = null, RefCode = ..., etc.
            }; await quiz_repo.InsertQuizSessionAsync(session);
        }
        public async Task<QuizSession?> GetByTokenAsync(Guid token)
        {
            return await quiz_repo.GetByTokenAsync(token);
        }

        public async Task MarkTokenAsUsedAsync(Guid token)
        {
            await quiz_repo.MarkTokenAsUsedAsync(token);
        }
        public async Task<Guid> GenerateAccessTokenAsync(string email)
        {
            var session = await quiz_repo.GetByEmailAsync(email);
            if (session == null)
                throw new KeyNotFoundException("No quiz session found for the given email.");

            var token = Guid.NewGuid();

            await quiz_repo.MarkSessionAsPaidAsync(email, token);
            return token;
        }
        public async Task<bool> SessionExistsAsync(string email)
        {
            return await quiz_repo.SessionExistsAsync(email);
        }
    }
}

