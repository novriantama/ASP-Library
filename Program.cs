using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using Npgsql;
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

// Configure JWT Authentication
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "super_secret_jwt_key_that_is_at_least_32_bytes_long_12345!";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "ASP-Library";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "ASP-Library-Users";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Build / Resolve PostgreSQL connection string from .env / environment variables
var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

string connectionString;
if (!string.IsNullOrWhiteSpace(rawConnectionString) && !rawConnectionString.Contains("[Password]"))
{
    // Substitute placeholders from .env variables
    connectionString = rawConnectionString
        .Replace("{DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
        .Replace("{DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "5432")
        .Replace("{DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "library_db")
        .Replace("{DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "postgres")
        .Replace("{DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres");
}
else
{
    // Fallback: build connection string directly from individual .env variables
    var csBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost",
        Port = int.TryParse(Environment.GetEnvironmentVariable("DB_PORT"), out var port) ? port : 5432,
        Database = Environment.GetEnvironmentVariable("DB_NAME") ?? "library_db",
        Username = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres",
        Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres"
    };
    connectionString = csBuilder.ConnectionString;
}

// Store resolved connection string in configuration for any dependent services
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

// DbContext with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repositories & Services
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
