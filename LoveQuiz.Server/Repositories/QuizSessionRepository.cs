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
        catch (PostgresException ex) when (ex.SqlState == "23505") // unique_violation
        {
            throw new ArgumentException("Această adresă de email a fost deja folosită.");
        }
    }

    public async Task<QuizSession?> GetByEmailAsync(string email)
    {
        const string sql = "SELECT * FROM quiz_sessions WHERE email = @Email;";
        return await _db.QueryFirstOrDefaultAsync<QuizSession>(sql, new { Email = email });
    }

    public async Task MarkTokenAsUsedAsync(Guid token)
    {
        const string sql = @"
        UPDATE quiz_sessions
        SET token_used = TRUE
        WHERE access_token = @Token;
    ";

        await _db.ExecuteAsync(sql, new { Token = token });
    }
    public async Task<QuizSession?> GetByTokenAsync(Guid token)
    {
        const string sql = @"
        SELECT * FROM quiz_sessions 
        WHERE access_token = @Token AND token_used = FALSE;
    ";

        return await _db.QueryFirstOrDefaultAsync<QuizSession>(sql, new { Token = token });
    }
    public async Task MarkSessionAsPaidAsync(string email, Guid token)
    {
        const string sql = @"
        UPDATE quiz_sessions
        SET converted = TRUE,
            access_token = @Token,
            token_used = FALSE
        WHERE email = @Email;
    ";

        await _db.ExecuteAsync(sql, new { Email = email, Token = token });
    }
}
