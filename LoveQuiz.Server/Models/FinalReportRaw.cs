namespace LoveQuiz.Server.Models
{

    //GIVEN BY OPEN AI
    public class FinalReportRaw
    {
        public int AttachmentStyleId { get; set; }
        // Just the ID — you’ll map it to all text/images backend-side

        public List<bool> EmotionalNeedsMet { get; set; } = new();
        // Index-based mapping to predefined emotional needs list

        public List<LoveTrait> Aspects { get; set; } = new();
        // Includes aspect name, description, and score

        public int ToxicityLevel { get; set; }
        // 0–100 range

        public List<string> AdviceList { get; set; } = new();
        // Final practical tips, already written by AI
    }
}
