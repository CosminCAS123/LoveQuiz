
using LoveQuiz.Server.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace LoveQuiz.Server.Services;

    public class OpenAIReportService
    {
       private readonly HttpClient _httpClient;
       private readonly IConfiguration _config;
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
            model = "gpt-4o-mini", // or "gpt-4o" later
            messages = new[]
           {
new
{
    role = "system",
    content = "Ești un psiholog specializat în relații de cuplu. Răspunsul tău trebuie să fie inteligent emoțional, empatic și scris în limba română. Formatează răspunsul strict ca obiect JSON, fără explicații, markdown sau text suplimentar."
},                new
{
    role = "user",
    content = prompt
}
            },
            temperature = 0.8,
            max_tokens = 800
        };
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
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
        using var root = JsonDocument.Parse(json);
        var content = root
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        var cleanJson = ExtractJson(content!);
        var report = JsonSerializer.Deserialize<FinalReport>(cleanJson!,
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

