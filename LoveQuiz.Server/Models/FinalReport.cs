namespace LoveQuiz.Server.Models
{
    public class FinalReport
    {
        public string Title { get; set; } = string.Empty;
        // Enhanced version of the free title (e.g. "Teama de abandon domină relația")

        public string Summary { get; set; } = string.Empty;
        // 2–3 sentence overview of the couple’s emotional dynamic

        public int ToxicityLevel { get; set; }
        // 1–5 scale, shown with emoji or color (e.g., 🟢 to 🔴)

        public string CompatibilityVerdict { get; set; } = string.Empty;
        // Short sentence like "Vă potriviți, dar cu eforturi constante"

        public List<string> KeyInsights { get; set; } = new();
        // Personal observations based on answers

        public List<string> AdviceList { get; set; } = new();
        // Concrete tips for improvement

       
    }
}
