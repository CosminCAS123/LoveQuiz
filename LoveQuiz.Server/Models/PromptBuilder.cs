using LoveQuiz.Server.Services;
using System.Text;

namespace LoveQuiz.Server.Models
{
    public static class PromptBuilder
    {
        public static string BuildPrompt(List<QuizSubmissionTextDto> submissions)
        {
            
            var sb = new StringBuilder();

            sb.AppendLine("Bazându-te pe următoarele răspunsuri dintr-un quiz de cuplu, generează o analiză psihologică în format JSON, cu următoarea structură:");
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
            sb.AppendLine("Răspunsuri:");

            int index = 1;
            foreach(var pair in submissions)
            {
                var question = pair.QuestionText;
                var answer = pair.AnswerText;

                sb.AppendLine($"Q{index}: \"{question}\"");
                sb.AppendLine($"A{index}: \"{answer}\"\n");
                index++;
            }
           
            return sb.ToString();
        }
    }


}
