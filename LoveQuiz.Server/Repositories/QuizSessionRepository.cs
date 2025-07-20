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




 }
