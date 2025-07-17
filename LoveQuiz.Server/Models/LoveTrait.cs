namespace LoveQuiz.Server.Models
{
    public class LoveTrait
    {
        public string Aspect { get; set; } = string.Empty;
        // e.g. "Emotional Dependency"

        public int Score { get; set; }
        // 0–100, for charts or color-coded indicators

        public string Description { get; set; } = string.Empty;

    }
}
