using Microsoft.AspNetCore.Mvc;
using LoveQuiz.Server.Models;
using System.Text.Json;
using LoveQuiz.Server.Services;
using System.Text;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;



namespace LoveQuiz.Server.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController: ControllerBase
    {
        private const string API_KEY_SANDBOX = "ZnicPZDvXXrekQNlzHNCW26taBBt4SncvTyhzHdoxDPj6aEOoaFVJ_JDNwk=";
        private const string POS_SIGNATURE_SANDBOX = "33CH-UFEF-OVGF-QEXQ-TDB9";
        private readonly QuizService _quizService;
        private readonly IWebHostEnvironment _env;
        private static readonly Guid DevBypassToken = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly PdfGenerationService _pdfGenerationService;

        public QuizController(QuizService quizService, IWebHostEnvironment env , PdfGenerationService pdfGenerationService)
        {
            this._quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
            _env = env;
            _pdfGenerationService = pdfGenerationService;
        }


        [HttpGet("session/{sessionId}/status")]
        public async Task<ActionResult<object>> GetSessionStatus(string sessionId)
        {
            if (!Guid.TryParse(sessionId, out var guid))
                return BadRequest("Invalid session ID format.");

            var session = await _quizService.GetBySessionIdAsync(guid);
            if (session == null)
                return NotFound();

            return Ok(new { converted = session.Converted, sessionId = session.Id });
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
                    var session = await _quizService.GetBySessionIdAsync(dto.SessionId);
                    if (session == null || !session.Converted)
                        return Unauthorized("Invalid or unpaid session.");

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

        [HttpPost("payment/start")]
        public async Task<IActionResult> StartPayment([FromBody] string sessionId)
        {
            if (!Guid.TryParse(sessionId, out var guid))
                return BadRequest("Invalid session ID");

            // (Optional) check session exists in DB here

            var payload = new
            {
                config = new
                {
                    emailTemplate = "confirm",
                    notifyUrl = "https://incunabular-christy-nondigestibly.ngrok-free.dev/api/quiz/netopia/confirm"
, // TODO: set when tunneling
                    redirectUrl = "https://localhost:5173/payment/return",
                    language = "ro"
                },
                payment = new
                {
                    options = new { installments = 1, bonus = 0 },
                    instrument = new { type = "card" } // important: no PAN/CVV here
                },
                order = new
                {
                    posSignature = POS_SIGNATURE_SANDBOX,
                    dateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    description = "LoveQuiz Full Report",
                    orderID = sessionId,      // your GUID as string
                    amount = 49.00m,
                    currency = "RON",
                    billing = new
                    {
                        email = "test@example.com", // TODO: read from your session
                        phone = "0000000000",
                        firstName = "Unknown",
                        lastName = "Unknown",
                        city = "Unknown",
                        country = 642,
                        state = "-",
                        postalCode = "-",
                        details = "-"
                    }
                }
            };

            using var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", API_KEY_SANDBOX);


            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://secure.sandbox.netopia-payments.com/payment/card/start", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            // Prefer payment.paymentURL
            if (root.TryGetProperty("payment", out var pay) &&
                pay.TryGetProperty("paymentURL", out var purl) &&
                purl.ValueKind == JsonValueKind.String)
            {
                return Ok(new { sessionId, redirectUrl = purl.GetString() });
            }

            // Fallback: customerAction.url
            if (root.TryGetProperty("customerAction", out var ca) &&
                ca.TryGetProperty("url", out var curl) &&
                curl.ValueKind == JsonValueKind.String)
            {
                return Ok(new { sessionId, redirectUrl = curl.GetString() });
            }

            // Otherwise, just return raw
            return Content(responseBody, "application/json");
        }
        [AllowAnonymous]
        [HttpPost("netopia/confirm")]
        public async Task<IActionResult> NetopiaConfirm([FromBody] JsonElement body)
        {
            try
            {
                Console.WriteLine("[NETOPIA IPN] " + body.GetRawText());

                string? orderId = null;
                int status = 0;

                // code may be string, number, or absent in IPN
                string? code = null;

                if (body.TryGetProperty("order", out var order) &&
                    order.TryGetProperty("orderID", out var orderIdEl) &&
                    orderIdEl.ValueKind == JsonValueKind.String)
                {
                    orderId = orderIdEl.GetString();
                }

                if (body.TryGetProperty("payment", out var payment) &&
                    payment.TryGetProperty("status", out var statusEl) &&
                    statusEl.ValueKind == JsonValueKind.Number)
                {
                    status = statusEl.GetInt32();
                }

                if (body.TryGetProperty("error", out var err) &&
                    err.TryGetProperty("code", out var codeEl))
                {
                    if (codeEl.ValueKind == JsonValueKind.String)
                        code = codeEl.GetString();
                    else if (codeEl.ValueKind == JsonValueKind.Number)
                        code = codeEl.GetInt32().ToString("00"); // normalize numeric 0 -> "00"
                }

                if (Guid.TryParse(orderId, out var sessionGuid))
                {
                    // From docs:
                    // - status 3 + code "00" => approved
                    // - status 15 + code "100" => 3DS step (ignore here; not approved yet)
                    // - status 5 only counts if code "00"; other codes => not completed

                    bool isApproved =
                        (status == 3 && (code == null || code == "00"))   // accept 3 even if code missing
                        || (status == 5 && code == "00");

                    if (isApproved)
                    {
                        await _quizService.SetConvertedAsync(sessionGuid, true);
                    }
                }

                return Ok(new { received = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[NETOPIA IPN ERROR] {ex}");
                return Ok(new { received = false });
            }
        }

    }


}

