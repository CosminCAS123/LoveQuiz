
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
       private const int MAX_TOKENS = 1000; // Default max tokens for OpenAI API
    const string SYSTEM_MESSAGE = @"
Ești un psiholog de cuplu empatic, direct și atent la nuanțe. Analizezi răspunsuri la un test de cuplu și returnezi un obiect JSON valid, corespunzător structurii FinalReportRaw, fără explicații suplimentare. 

Formatul exact este:

- attachmentStyleId: un număr întreg între 1 și 5 care indică stilul de atașament dominant.
- emotionalNeedsMet: o listă de 10 valori booleene (true sau false), în aceeași ordine ca lista fixă de nevoi emoționale. Nu menționa numele nevoilor.
- aspects: o listă de 4 obiecte LoveTrait care conțin:
  - aspect: numele unei teme psihologice (ex: încredere, comunicare),
  - score: un scor reprezentativ între 0 și 10,
  - description: o descriere emoțională a modului în care acest aspect influențează relația, 2-3 propozitii.
- toxicityLevel: un scor între 0 și 100 care reflectă nivelul de toxicitate al relatiei.
- adviceList: o listă de 4-5 sfaturi practice, empatice și clare pentru îmbunătățirea relației.

Scrie exclusiv JSON valid, fără text suplimentar sau explicații. Scrie totul în limba română. Nu inventa lucruri.
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

        //CONVERT
        return new FinalReport
        {
            AttachmentStyle = attachmentStyle,
            MetNeeds = metNeeds,
            UnmetNeeds = unmetNeeds,
            Aspects = rawReport.Aspects,
            ToxicityLevel = rawReport.ToxicityLevel,
            AdviceList = rawReport.AdviceList,
            ToxicHabitsSection = toxicHabits
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

