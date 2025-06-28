using Microsoft.AspNetCore.Mvc;
using LoveQuiz.Server.Models;
using System.Text.Json;


namespace LoveQuiz.Server.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private const string jsonPath = "Data/quiz_data.json";


        [HttpGet("questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, jsonPath);

                if (!System.IO.File.Exists(filePath))
                    return NotFound($"File {jsonPath} not found.");

                var json = await System.IO.File.ReadAllTextAsync(filePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var questions = System.Text.Json.JsonSerializer
                                 .Deserialize<IEnumerable<QuestionDto>>(json, options);

                if (questions == null || !questions.Any())
                    return NoContent();           // 204 — file empty / no questions

                return Ok(questions);
            }
            catch (System.Text.Json.JsonException)
            {
                return BadRequest("Malformed JSON.");
            }
            catch (Exception ex)
            {
                // log ex here ⬅️ (don’t expose raw details in production)
                return StatusCode(500, $"Internal server error : {ex.Message}");
            }
        }

    }
}
