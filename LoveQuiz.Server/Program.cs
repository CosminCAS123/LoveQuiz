using Dapper;
using Npgsql;
using System.Data;
using LoveQuiz.Server.Services;
using AspNetCoreRateLimit;
using QuestPDF.Infrastructure;
var builder = WebApplication.CreateBuilder(args);


//builder.Configuration.AddEnvironmentVariables();


QuestPDF.Settings.License = LicenseType.Community;
builder.Services.AddControllers();
builder.Services.AddHttpClient();

//for IP RATE LIMITING

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<QuizService>();
builder.Services.AddSingleton<QuizQuestionsCache>();
builder.Services.AddScoped<QuizSessionRepository>();
builder.Services.AddScoped<OpenAIReportService>();

builder.Services.AddScoped<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("Supabase");

 

    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Connection string 'Supabase' not found.");

    return new NpgsqlConnection(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", p => p
        .WithOrigins("https://localhost:5173") // frontend
        .AllowAnyHeader()
        .AllowAnyMethod());
});
Console.WriteLine("Supabase connection: " + builder.Configuration.GetConnectionString("Supabase"));

var app = builder.Build();



app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//DO WHEN APP GOES LIVE

/*if (app.Environment.IsProduction())
{
    app.UseIpRateLimiting();
}
*/ 
app.UseHttpsRedirection();

app.UseCors("Dev");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
