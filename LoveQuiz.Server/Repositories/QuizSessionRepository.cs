using System.Data;
using System.Text.Json;
using Dapper;

public class QuizSessionRepository
{
    private readonly IDbConnection _db;

    public QuizSessionRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task InsertTestRowAsync()
    {
        const string sql = @"
        INSERT INTO quiz_sessions (
            email,
            gender,
            answers_json,
            stripe_ref,
            payment_status,
            created_at
        )
        VALUES (
            'test@example.com',
            'test-gender',
            '[{""qId"": 1, ""aId"": 1}]',
            'local_test',
            'Pending',
            NOW()
        );";

        await _db.ExecuteAsync(sql);
    }



}
