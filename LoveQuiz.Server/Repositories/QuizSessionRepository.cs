using System.Data;
using System.Text.Json;
using Dapper;
using LoveQuiz.Server.Models;

 public class QuizSessionRepository
{
    private readonly IDbConnection _db;

    public QuizSessionRepository(IDbConnection db)
    {
        _db = db;
   }


    public async Task InsertQuizVisitAsync(QuizVisit visit)
    {
        const string sql = @"
        INSERT INTO quiz_visits (email, gender, converted, created_at)
        VALUES (@Email, @Gender, @Converted, @CreatedAt);
    ";

        await _db.ExecuteAsync(sql, new
        {
            visit.Email,
            visit.Gender,
            visit.Converted,
            visit.CreatedAt
        });
    }

    public async Task InsertPaidQuizAsync(PaidQuiz quiz)
    {
        var sql = @"
            INSERT INTO paid_quizzes (
                email, gender, payment_status, phone_number, answers_json, created_at
            )
            VALUES (
                @Email, @Gender, @PaymentStatus, @PhoneNumber, @AnswersJson, @CreatedAt
            )";

        await _db.ExecuteAsync(sql, quiz);
    }
}
