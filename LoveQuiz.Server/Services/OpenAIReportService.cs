
using LoveQuiz.Server.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace LoveQuiz.Server.Services;

    public class OpenAIReportService
    {
       private readonly HttpClient _httpClient;
       private readonly IConfiguration _config;
       private const float TEMP = 0.8f; // Default temperature for OpenAI API
       private const int MAX_TOKENS = 1400; // Default max tokens for OpenAI API
    const string SYSTEM_MESSAGE = @"
Ești un psiholog de cuplu empatic și concis. Primești răspunsurile unui chestionar și trebuie să întorci EXCLUSIV un JSON VALID care corespunde exact clasei FinalReportRaw, fără explicații, fără markdown, fără text în plus.

STRUCTURĂ JSON (chei și tipuri EXACTE):
{
  ""AttachmentStyleId"": number (1..5),
  ""EmotionalNeedsMet"": [10 booleans în ORDINEA de mai jos],
  ""Aspects"": [
    { ""Aspect"": string, ""Score"": number (0..10), ""Description"": string (2–3 propoziții) },
    { ... } (exact 4 obiecte în total)
  ],
  ""ToxicityLevel"": number (0..100),
  ""AdviceList"": [4–5 string-uri, ~20 cuvinte fiecare]
}

MAPARE STILURI DE ATAȘAMENT (alege UN SINGUR ID):
1 = Anxios–preocupat
2 = Evitant–dezangajat
3 = Preocupat–controlator
4 = Manipulativ–defensiv
5 = Echilibrat

ORDINEA FIXĂ PENTRU EmotionalNeedsMet (NU scrie numele în JSON, doar true/false în această ordine):
1. Să fii înțeles
2. Să te simți în siguranță
3. Să fii valorizat
4. Responsivitate emoțională
5. Încredere
6. Sprijin în vulnerabilitate
7. Afecțiune & atingere
8. Autonomie & spațiu personal
9. Claritate în comunicare
10. Scop comun

CERINȚE DE CONȚINUT:
- Aspects: 4 teme relevante (ex. Comunicare, Încredere, Asertivitate, Autocontrol, Empatie, Gestionarea conflictelor etc.), fiecare cu Score 0..10 și Description (2–3 propoziții, ton empatic, concret, în română).
- AdviceList: 4–5 recomandări scurte (~20 cuvinte), acționabile, clare, fără clișee.
- ToxicityLevel: 0..100, coerent cu tonul general.

IMPORTANT:
- Limba: română.
- Fără date sensibile/diagnostic clinic.
- Răspunde DOAR cu JSON valid conform structurii de mai sus.
";






    public OpenAIReportService(HttpClient http, IConfiguration config)
        {
        this._httpClient = http;
        this._config = config;

        }   










    public async Task<FinalReport> GetReportOpenAIAsync(string prompt)
    {
        var apiKey = _config["OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI API key not found.");

        var requestBody = new
        {
            model = "gpt-4o-mini",
            // ensures strict JSON output
            temperature = TEMP,
            max_tokens = MAX_TOKENS,
            messages = new[]
     {
        new
        {
            role = "system",
            content = SYSTEM_MESSAGE
        },
        new
        {
            role = "user",
            content = prompt
        }
    }
        };

        var raw_report = await GenerateFinalReportRawAsync(requestBody, apiKey);

        var final_report = ConvertRawToFinalReport(raw_report);

        return final_report;

    }










    private async Task<FinalReportRaw> GenerateFinalReportRawAsync(object requestBody, string apiKey)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = JsonContent.Create(requestBody);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("OpenAI API key is invalid or unauthorized.");
        }
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidOperationException("Received empty response from OpenAI.");
        }

        using var document = JsonDocument.Parse(json);
        var resultElement = document.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content");
        var contentString = resultElement.GetString();

        var cleanJson = ExtractJson(contentString!);

        var rawReport = JsonSerializer.Deserialize<FinalReportRaw>(cleanJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return rawReport!;
    }
    private FinalReport ConvertRawToFinalReport(FinalReportRaw rawReport)
    {
        //EMOTIONAL NEEDS
        var allNeeds = StaticInfo.EmotionalNeeds;
        var metNeeds = new List<EmotionalNeed>();
        var unmetNeeds = new List<EmotionalNeed>();
        for (int i = 0;  i < allNeeds.Count; i++)
        {
            if (rawReport.EmotionalNeedsMet[i])
                metNeeds.Add(allNeeds[i]);
            else
                unmetNeeds.Add(allNeeds[i]);
        }

        var attachmentStyle = StaticInfo.AttachmentStyles[rawReport.AttachmentStyleId]; //GET ATTACHEMENTSTYLEINFO
        var toxicHabits = StaticInfo.ToxicHabitsByStyle[rawReport.AttachmentStyleId]; //GET TOXICHABITS BY ID
        var shortText = StaticInfo.ShortTexts[rawReport.AttachmentStyleId];//GET SHORT TEXT BY ATTACHEMENTSTYLEID

        //CONVERT
        return new FinalReport
        {
            AttachmentStyle = attachmentStyle,
            MetNeeds = metNeeds,
            UnmetNeeds = unmetNeeds,
            Aspects = rawReport.Aspects,
            ToxicityLevel = rawReport.ToxicityLevel,
            AdviceList = rawReport.AdviceList,
            ToxicHabitsSection = toxicHabits,
            AttachmentStyleShortText = shortText
        };
    }
    private static string ExtractJson(string content)
    {
        var start = content.IndexOf('{');
        var end = content.LastIndexOf('}');

        if (start == -1 || end == -1 || end <= start)
            throw new FormatException("Could not find valid JSON in OpenAI response.");

        return content.Substring(start, end - start + 1);
    }


}

