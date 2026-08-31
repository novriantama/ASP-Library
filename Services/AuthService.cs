using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ASP_Library.Dtos;
using ASP_Library.Entities;
using ASP_Library.Repositories;

namespace ASP_Library.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByUsernameOrEmailAsync(request.UsernameOrEmail);
        if (user == null)
        {
            _logger.LogWarning("Login failed: User '{Identifier}' not found", request.UsernameOrEmail);
            return null;
        }

        bool isPasswordValid = VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            _logger.LogWarning("Login failed: Incorrect password for user '{Username}'", user.Username);
            return null;
        }

        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") 
            ?? _configuration["Jwt:Secret"] 
            ?? "super_secret_jwt_key_that_is_at_least_32_bytes_long_12345!";

        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
            ?? _configuration["Jwt:Issuer"] 
            ?? "ASP-Library";

        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
            ?? _configuration["Jwt:Audience"] 
            ?? "ASP-Library-Users";

        var expirationMinutesStr = Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES") 
            ?? _configuration["Jwt:ExpirationMinutes"] 
            ?? "60";

        int expirationMinutes = int.TryParse(expirationMinutesStr, out var mins) ? mins : 60;
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = expiresAt,
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        _logger.LogInformation("User '{Username}' logged in successfully", user.Username);

        return new LoginResponseDto
        {
            Token = tokenString,
            TokenType = "Bearer",
            ExpiresIn = expirationMinutes * 60,
            User = new UserResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            }
        };
    }

    private static bool VerifyPassword(string inputPassword, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(inputPassword))
        {
            return false;
        }

        // If stored as BCrypt hash
        if (storedHash.StartsWith("$2a$") || storedHash.StartsWith("$2b$") || storedHash.StartsWith("$2y$"))
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
            }
            catch
            {
                return false;
            }
        }

        // Fallback for plain-text password match
        return inputPassword == storedHash;
    }
}
