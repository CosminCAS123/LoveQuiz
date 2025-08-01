﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly IWebHostEnvironment _env;
        private static readonly Guid DevBypassToken = Guid.Parse("00000000-0000-0000-0000-000000000001");


        public QuizController(QuizService quizService, IWebHostEnvironment env)
        {
            this._quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
            _env = env;
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





    }


}

