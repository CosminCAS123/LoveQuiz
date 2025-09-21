using Microsoft.AspNetCore.Mvc;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using LoveQuiz.Server.Models;
using QuestPDF.Infrastructure;
using LoveQuiz.Server.Services;

#if DEBUG
using QuestPDF.Previewer;
#endif

[Route("api/[controller]")]
[ApiController]
public  class PdfGenerationController : ControllerBase
{
#if DEBUG
    [HttpGet("preview")]    
    public IActionResult Preview()  
    {
        var finalReport = new FinalReport
        {
            AttachmentStyle = StaticInfo.AttachmentStyles[1],

            MetNeeds = new List<EmotionalNeed>
            {
               StaticInfo.EmotionalNeeds[0],
               StaticInfo.EmotionalNeeds[1],
               StaticInfo.EmotionalNeeds[2],
               StaticInfo.EmotionalNeeds[3]


            },

            UnmetNeeds = new List<EmotionalNeed>
            {
                StaticInfo.EmotionalNeeds[4],
               StaticInfo.EmotionalNeeds[5],
               StaticInfo.EmotionalNeeds[6],
               StaticInfo.EmotionalNeeds[7]
            },
            Aspects = new List<LoveTrait>
            {
                new LoveTrait
                {
                    Aspect = "Empatie",
                    Description = "Îți pasă de sentimentele celuilalt și încerci să înțelegi reacțiile lui. Această empatie poate crea o atmosferă mai caldă în relații",
                    Score = 8
                },
                new LoveTrait
                {
                    Aspect = "Comunicatie",
                    Description = "Îți exprimi gândurile și sentimentele, chiar dacă uneori te simți atacată. E un pas important spre crearea unei legături mai profunde",
                    Score = 8
                },
                new LoveTrait
                {
                    Aspect = "Autocontrol",
                    Description = "În momentele de tensiune, reușești să te abții de la reacții impulsive, semn că îți dorești să rezolvi conflictele cu calm",
                    Score = 7
                },
                new LoveTrait
                {
                    Aspect = "Asertivitate",
                    Description = "Deși îți exprimi dorințele, uneori te simți nesigură sau neînțeleasă. Cultivarea încrederii în sine te va ajuta să comunici mai eficient",
                    Score = 10
                }
            },


            ToxicityLevel = 60,
            AttachmentStyleShortText = StaticInfo.ANXIOS_PREOCUPAT_SUBTITLE,

            AdviceList = new List<string>
{
    "Construiește încrederea prin mici gesturi zilnice și discuții deschise, pentru a crea o bază solidă în relație.",
    "Exersează ascultarea activă și răspunde cu empatie, chiar și atunci când opiniile voastre diferă semnificativ.",
    "Rezervă timp regulat pentru activități comune care vă aduc bucurie și vă întăresc legătura emoțională.",
    "Învață să exprimi nevoile tale clar și fără reproșuri, pentru a evita acumularea de tensiuni nesănătoase."
},

            ToxicHabitsSection = StaticInfo.ToxicHabitsByStyle[3]


        };




        var doc = new FinalReportDocumentPdf(finalReport);
        doc.ShowInCompanion(12500);
        return Ok("PDF sent to Companion");





    }
#else
    [HttpGet("preview")]
    public IActionResult Disabled() => NotFound();
#endif
}