using ASP_Library.Dtos;

namespace ASP_Library.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}
