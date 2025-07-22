
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
    const string SYSTEM_MESSAGE =
"Ești un psiholog de cuplu empatic, direct și atent la nuanțele emoționale. Primești răspunsuri personale la un chestionar despre comportamente în relații și trebuie să generezi un raport în format JSON valid, care să conțină următoarele câmpuri exacte:\n\n" +
"- title: un titlu emoțional și expresiv, inspirat din răspunsuri (nu robotic, nu general, nu plat)\n" +
"- summary: 2–3 fraze calde și personale despre stilul relațional al persoanei; scrie ca și cum i-ai vorbi direct\n" +
"- toxicityLevel: număr între 0 și 100 care reflectă nivelul de toxicitate în comportamentele persoanei care a completat quizul (nu al relației, nu al partenerei)\n" +
"- aspects: listă cu MINIM 4 trăsături (fără limită superioară), fiecare având:\n" +
"    - aspect: numele trăsăturii observate (ex: Încredere, Empatie, Comunicare etc.)\n" +
"    - score: între 0 și 10, variat (evită să folosești aceleași scoruri repetitiv)\n" +
"    - description: 2–4 propoziții clare, calde și personale. Nu fi robotic. Vorbește direct persoanei. Evită expresii impersonale precum „se observă că” sau „utilizatorul pare să”. Nu repeta „poate” în mod excesiv.\n" +
"- adviceList: listă cu 4–6 sfaturi practice, empatice, scrise într-un ton blând și încurajator\n\n" +
"IMPORTANT: Nu include absolut nimic în afara obiectului JSON. Nu adăuga explicații suplimentare. Nu include propoziții înainte sau după răspuns. Scrie exclusiv JSON valid.";





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










    public async Task<FinalReportRaw> GenerateFinalReportRawAsync(object requestBody, string apiKey)
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

