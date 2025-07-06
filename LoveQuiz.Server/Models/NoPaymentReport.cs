namespace LoveQuiz.Server.Models
{
    public class NoPaymentReport
    {
        public string Title { get; set; } = string.Empty; //short title for the report
        public string Teaser { get; set; } = string.Empty; //short hook sentence

        public int SeverityLevel { get; set; } = 0; //EMOJI LEVEL 1-5 scale



    }
}
