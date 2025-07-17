namespace LoveQuiz.Server.Models
{
    public class FinalReport
    {
        public string Title { get; set; } = string.Empty;
        // Custom, emotionally-resonant title (e.g. "The Over-Giver: When Love Becomes Self-Sacrifice")

        public string Summary { get; set; } = string.Empty;
        // 2–3 sentence overview of the user's emotional dynamic

        public int ToxicityLevel { get; set; }
        // 0–100 scale; frontend maps this to labels/emojis (e.g., 🟢 0–20, 🔴 61–80)

        public string CompatibilityVerdict { get; set; } = string.Empty;
        // One-liner judgment (e.g., "You match, but only with conscious effort")

        public List<LoveTrait> Aspects { get; set; } = new();
        // Detailed breakdown per aspect (e.g., emotional dependency, communication)

        public List<string> AdviceList { get; set; } = new();
        // Specific tips or exercises to improve relationship health
    }
}
