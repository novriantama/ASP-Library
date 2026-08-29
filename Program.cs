using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using ASP_Library.Data;
using ASP_Library.Repositories;
using ASP_Library.Services;

// Load environment variables from .env file if present
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Configure Host & Port from environment variables
var appHost = Environment.GetEnvironmentVariable("APP_HOST") ?? "0.0.0.0";
var appPort = Environment.GetEnvironmentVariable("APP_PORT") ?? "8080";
builder.WebHost.UseUrls($"http://{appHost}:{appPort}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Build PostgreSQL connection string from environment variables
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "library_db";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

// DbContext with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repositories & Services
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
