using Dapper;
using Npgsql;
using System.Data;
using LoveQuiz.Server.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<QuizService>();
builder.Services.AddSingleton<QuizQuestionsCache>();
builder.Services.AddScoped<QuizSessionRepository>();
builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(
       builder.Configuration.GetConnectionString("Supabase") ?? 
          throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", p => p
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();



app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseCors("Dev");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
