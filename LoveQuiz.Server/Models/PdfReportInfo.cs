namespace LoveQuiz.Server.Models
{
    public class PdfReportInfo
    {
        public byte[] ByteArray { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
    }
}
