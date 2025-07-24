using LoveQuiz.Server.Services;
using System.Text;

namespace LoveQuiz.Server.Models
{
    public static class PromptBuilder
    {
        public static string BuildPrompt(List<QuizSubmissionTextDto> submissions)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Acestea sunt răspunsurile oferite de un utilizator la chestionarul despre comportamente și reacții în relații de cuplu.\n");

            int index = 1;
            foreach (var pair in submissions)
            {
                sb.AppendLine($"Întrebare {index}: \"{pair.QuestionText}\"");
                sb.AppendLine($"Răspuns {index}: \"{pair.AnswerText}\"\n");
                index++;
            }

            sb.AppendLine("Te rog să analizezi aceste răspunsuri pentru a identifica tiparele emoționale și comportamentale ale utilizatorului.");
            sb.AppendLine("Fă o analiză empatică și creează un raport psihologic complet, în format JSON, conform instrucțiunilor din system message.");

            return sb.ToString();
        }



    }


}
