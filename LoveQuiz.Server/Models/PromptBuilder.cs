using LoveQuiz.Server.Services;
using System.Text;

namespace LoveQuiz.Server.Models
{
    public static class PromptBuilder
    {
        public static string BuildPrompt(List<QuizSubmissionTextDto> submissions)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Răspunsurile utilizatorului la chestionar:");

            for (int i = 0; i < submissions.Count; i++)
            {
                var q = submissions[i];
                sb.AppendLine($"Întrebare {i + 1}: \"{q.QuestionText}\"");
                sb.AppendLine($"Răspuns {i + 1}: \"{q.AnswerText}\"");
            }

            // Niciun alt text: system message conține toate regulile/ordinea/ID-urile.S
            return sb.ToString();
        }



    }


}
