
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
    private const string SYSTEM_MESSAGE =
      "Ești un psiholog de cuplu sincer și empatic. Analizează răspunsurile și generează un raport în format JSON valid cu câmpurile: title, summary, toxicityLevel (0–100), compatibilityVerdict, aspects (listă de LoveTrait cu aspect, score, description), adviceList (listă de stringuri). Scrie în română. Fără text suplimentar.";
    public OpenAIReportService(HttpClient http, IConfiguration config)
        {
        this._httpClient = http;
        this._config = config;

        }   
    public async Task<FinalReport> GetReportOpenAIAsync(string prompt)
    {
        var apiKey = _config["OpenAI:ApiKey"]
          ?? throw new InvalidOperationException("OpenAI API key not found.");

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

        // ✅ Directly parse JSON if using response_format: "json"
        using var document = JsonDocument.Parse(json);
        var resultElement = document.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content");
        var contentString = resultElement.GetString();
        Console.WriteLine("RAW JSON from AI:");
        Console.WriteLine(contentString);

        var cleanJson = ExtractJson(contentString!);

        var report = JsonSerializer.Deserialize<FinalReport>(cleanJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return report!;
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

