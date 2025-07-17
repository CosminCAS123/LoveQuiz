using Microsoft.AspNetCore.Mvc;
using LoveQuiz.Server.Models;
using System.Text.Json;
using LoveQuiz.Server.Services;



namespace LoveQuiz.Server.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController: ControllerBase
    {
        private readonly QuizService _quizService;
        public QuizController(QuizService quizService)
        {
            this._quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
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

        [HttpPost("free-report")]
        public ActionResult<NoPaymentReport>GetFreeReport([FromBody] IEnumerable<QuizSubmissionDto> submissions)
        {
            var report = _quizService.GetFreeReport(submissions);
            if (report == null)
            {
                return NotFound("No report available for the provided submissions.");
            }
            return Ok(report);
        }


        // [HttpPost("db-test")]
        // public async Task<IActionResult> DbTest()
        // {
        //     await _quizService.SaveSessionAsync();
        //     return Ok("✅ Test row inserted into Supabase.");
        // }

        [HttpPost("full-report")]
        public async Task<ActionResult<FinalReport>> GetFullReport([FromBody] List<QuizSubmissionDto> submissions)
        {
            try
            {
                var report = await _quizService.GetFullReportAsync(submissions);
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



    }
}
