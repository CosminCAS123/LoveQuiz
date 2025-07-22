namespace LoveQuiz.Server.Models
{
    public class AttachmentStyleInfo
    {
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty; // e.g., "ANXIOS-PREOCUPAT"
        public string Nickname { get; set; } = string.Empty; // e.g., "Furtuna Anxioasă"
        public string Summary { get; set; } = string.Empty; // "Ce înseamnă asta?" paragraph
    }
}
