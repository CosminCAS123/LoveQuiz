using Microsoft.AspNetCore.Mvc;
using LoveQuiz.Server.Models;
using System.Text.Json;
using LoveQuiz.Server.Services;
using System.Text;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;



namespace LoveQuiz.Server.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController: ControllerBase
        {

        private const string Netopia_API_KEY_NAME = "Netopia:ApiKey"; //PUT THESE VALUES IN AZURE ENVIRONMENT VARIABLES
        private const string Netopia_POS_NAME = "Netopia:PosSignature";//
        private const string Netopia_BASE_URL_NAME = "Netopia:BaseUrl";//
        private readonly QuizService _quizService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private static readonly Guid DevBypassToken = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly PdfGenerationService _pdfGenerationService;

        public QuizController(QuizService quizService, IWebHostEnvironment env , PdfGenerationService pdfGenerationService, IConfiguration config)
        {
            this._quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));

            _env = env;
            _pdfGenerationService = pdfGenerationService;
            _config = config;
        }


[HttpPost("payment/start")]
    public async Task<IActionResult> StartPayment()
    {
        using var client = new HttpClient();

        var payload = new
        {
            config = new
            {
                notifyUrl = "https://incunabular-christy-nondigestibly.ngrok-free.dev/api/quiz/ipn",
                redirectUrl = "https://localhost:5173/payment/return",
                language = "ro"
            },
            order = new
            {
                posSignature = _config["Netopia:PosSignature"],
                dateTime = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
                description = "LoveQuiz Full Report",
                orderID = Guid.NewGuid().ToString(),
                amount = 49,
                currency = "RON"
            }
        };

        var json = JsonSerializer.Serialize(payload);

        var request = new HttpRequestMessage(HttpMethod.Post,
            new Uri($"{_config["Netopia:BaseUrl"]}/payment/card/start"))
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    
        request.Headers.TryAddWithoutValidation("Authorization", _config["Netopia:ApiKey"]);

        using var response = await client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
            string? url = null;
            if (doc.RootElement.TryGetProperty("payment", out var p) &&
                p.TryGetProperty("paymentURL", out var u) &&
                u.ValueKind == JsonValueKind.String)
            {
                url = u.GetString();
            }

            if (string.IsNullOrWhiteSpace(url))
                return BadRequest(new { error = "Missing redirect url from provider", raw = body });

            return Ok(new { redirectUrl = url });

        }





        [HttpGet("session/{sessionId}/status")]
        public async Task<ActionResult<bool>> GetSessionStatus(string sessionId)
        {
            if (!Guid.TryParse(sessionId, out var guid))
                return BadRequest(false);

            var session = await _quizService.GetBySessionIdAsync(guid);
            if (session == null)
                return NotFound(false);

            return Ok(session.Converted); // <- just the boolean
        }

        [HttpGet("questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions([FromQuery] string gender)
        {
            try
            {
                var questions =  _quizService.GetAllQuestions(gender);

                if (!questions.Any())
                    return NoContent();

                
                return Ok(questions);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (JsonException)
            {
                return BadRequest("Malformed JSON.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //fuck it people can 

        [HttpPost("free-report")]
        public async Task<ActionResult<NoPaymentReport>>GetFreeReport([FromBody] IEnumerable<QuizSubmissionDto> submissions)
        {
           
            var report = _quizService.GetFreeReport(submissions);
            if (report == null)
            {
                return NotFound("No report available for the provided submissions.");
            }
            return Ok(report);
        }

            [HttpPost("full-report")]
            public async Task<ActionResult<FinalReport>> GetFullReport([FromBody] FullReportSessionRequestDto dto)
            {
                try
                {
                    /*var session = await _quizService.GetBySessionIdAsync(dto.SessionId);
                    if (session == null || !session.Converted)
                        return Unauthorized("Invalid or unpaid session.");
                    */ //already checked in payment component.
                    var report = await _quizService.GetFullReportAsync(dto.Submissions);
                    return Ok(report);
                }
                catch (UnauthorizedAccessException)
                {
                    return StatusCode(401, "API key invalid or unauthorized.");
                }
                catch (KeyNotFoundException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal error: {ex.Message}");
                }
            }

        [HttpPost("log-free-session")]
        public async Task<IActionResult> LogSession([FromBody] QuizSessionDto dto)
        {
            try
            {
                var sessionId = await _quizService.AddFreeQuizSessionAsync(dto);
                return Ok(new { message = "Session logged successfully", sessionId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"{ex.Message}" });
            }
        }


        //FOR NOW JUST RETURNS THE PDF, DOESNT SEND EMAIL OR ANYTHING
        [HttpPost("send-email-results")]
        public async Task<IActionResult> SendResults([FromBody]FinalReport finalReport , string email , CancellationToken ct)
        {
            if (finalReport is null)
                return BadRequest("finalReport is null");

            var bytes = await _pdfGenerationService.GenerateAsync(finalReport, ct);
            var fileName = $"LoveQuiz_Final_Report_{DateTime.UtcNow:yyyyMMdd_HHmm}.pdf";
            return File(bytes, "application/pdf", fileName);


        }

      


    }


}

