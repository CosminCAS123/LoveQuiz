using System.Data;
using System.Text.Json;
using Dapper;
using LoveQuiz.Server.Models;
using Npgsql;

public class QuizSessionRepository
{
    private readonly IDbConnection _db;



   //TOKNOW:AFTER PAYMENT, SEARCH FOR THE USER BY THEIR EMAIL(UNIQUE), AND TURN CONVERTED AND OTHER FIELDS ACCORDINGLY
    public QuizSessionRepository(IDbConnection db)
    {
        _db = db;
    }
    public async Task InsertQuizSessionAsync(QuizSession session)
    {
        const string sql = @"
        INSERT INTO quiz_sessions (id, email, gender, converted, created_at)
        VALUES (@Id, @Email, @Gender, @Converted, @CreatedAt);
    ";
        if (!session.Email.Contains("@") || session.Email.Length < 9) throw new ArgumentException("Adresa invalida.");


        try
        {
            await _db.ExecuteAsync(sql, new
            {
                session.Id,
                session.Email,
                session.Gender,
                session.Converted,
                session.CreatedAt
            });
        }
      catch (Exception ex) { Console.WriteLine(ex); }
    }



    public async Task<bool> SetConvertedAsync(Guid sessionId, bool value = true)
    {
        const string sql = @"
UPDATE quiz_sessions
SET converted = @Value,
    -- keep a timestamp if you later add this column:
    -- converted_at = CASE WHEN @Value = 1 THEN CURRENT_TIMESTAMP ELSE converted_at END
WHERE id = @SessionId;";

        var rows = await _db.ExecuteAsync(sql, new { SessionId = sessionId, Value = value });
        return rows > 0;
    }



    public async Task<QuizSession?> GetByEmailAsync(string email)
    {
        const string sql = "SELECT * FROM quiz_sessions WHERE email = @Email;";
        return await _db.QueryFirstOrDefaultAsync<QuizSession>(sql, new { Email = email });
    }
    public async Task<QuizSession?> GetBySessionIdAsync(Guid sessionId)
    {
        const string sql = "SELECT * FROM quiz_sessions WHERE id = @SessionId;";
        return await _db.QueryFirstOrDefaultAsync<QuizSession>(sql, new { SessionId = sessionId });
    }

}

