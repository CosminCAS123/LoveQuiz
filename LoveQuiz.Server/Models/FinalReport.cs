namespace LoveQuiz.Server.Models
{
    public class FinalReport
    {
        public AttachmentStyleInfo AttachmentStyle { get; set; } = new();
        // Contains id, label, nickname, and summary

        public List<EmotionalNeed> MetNeeds { get; set; } = new();
        public List<EmotionalNeed> UnmetNeeds { get; set; } = new();
        // Split based on AI's boolean list

        public List<LoveTrait> Aspects { get; set; } = new();
        // Each aspect with score and description

        public int ToxicityLevel { get; set; }
        // 0–100 scale

        public List<string> AdviceList { get; set; } = new();
        // Final actionable advice

        public ToxicHabitsSection ToxicHabitsSection { get; set; } = new();
        // Title + 4 predefined habits based on attachment style

        public string AverageAspectScore =>
    Aspects.Count == 0
        ? "0"
        : (Aspects.Average(a => a.Score) % 1 == 0
            ? Aspects.Average(a => a.Score).ToString("0")
            : Aspects.Average(a => a.Score).ToString("0.0"));

    }

}
