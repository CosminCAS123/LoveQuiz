using LoveQuiz.Server.Services;
using System.Text;

namespace LoveQuiz.Server.Models
{
    public static class PromptBuilder
    {
        public static string BuildPrompt(List<QuizSubmissionTextDto> submissions)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Iată răspunsurile:");

            int index = 1;
            foreach (var pair in submissions)
            {
                sb.AppendLine($"q:{index}: \"{pair.QuestionText}\"");
                sb.AppendLine($"a:{index}: \"{pair.AnswerText}\"\n");
                index++;
            }

            return sb.ToString();
        }


    }


}
