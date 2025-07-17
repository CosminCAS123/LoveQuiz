using Dapper;
using Npgsql;
using System.Data;
using LoveQuiz.Server.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Console.WriteLine("Loaded OpenAI Key: " + builder.Configuration["OpenAI:ApiKey"]?.Substring(0, 6) + "...");

builder.Services.AddControllers();
builder.Services.AddHttpClient(); // ?? This is the missing piece

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<QuizService>();
builder.Services.AddSingleton<QuizQuestionsCache>();
// builder.Services.AddScoped<QuizSessionRepository>();
builder.Services.AddScoped<OpenAIReportService>();

// builder.Services.AddScoped<IDbConnection>(sp =>
// {
//     var config = sp.GetRequiredService<IConfiguration>();
//     var connectionString = config.GetConnectionString("Supabase")
//         ?? throw new InvalidOperationException("Connection string 'Supabase' not found.");

//     return new NpgsqlConnection(connectionString);
// });
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
