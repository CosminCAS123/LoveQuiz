using LoveQuiz.Server.Services;
using System.Text;

namespace LoveQuiz.Server.Models
{
    public static class PromptBuilder
    {
        public static string BuildPrompt(List<QuizSubmissionTextDto> submissions)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Ești un psiholog relațional virtual. Bazându-te pe următoarele răspunsuri dintr-un quiz de cuplu, generează o analiză psihologică în format JSON cu această structură exactă:");
            sb.AppendLine(@"
{
  ""title"": ""string"",
  ""summary"": ""string"",
  ""toxicityLevel"": int,
  ""compatibilityVerdict"": ""string"",
  ""keyInsights"": [""string"", ""string""],
  ""adviceList"": [""string"", ""string""]
}
");
            sb.AppendLine("Scrie în română. Nu include nicio explicație în afara obiectului JSON. Iată răspunsurile:");

            int index = 1;
            foreach (var pair in submissions)
            {
                sb.AppendLine($"Întrebarea {index}: \"{pair.QuestionText}\"");
                sb.AppendLine($"Răspunsul {index}: \"{pair.AnswerText}\"\n");
                index++;
            }

            return sb.ToString();
        }

    }


}
