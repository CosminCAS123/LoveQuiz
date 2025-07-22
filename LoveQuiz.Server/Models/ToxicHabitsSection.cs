namespace LoveQuiz.Server.Models
{
    public class ToxicHabitsSection
    {
        public string Title { get; set; } = string.Empty; // e.g., "Toxicitate"
        public List<ToxicHabit> Habits { get; set; } = new();
    }
}
