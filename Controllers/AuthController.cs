using Microsoft.AspNetCore.Mvc;
using ASP_Library.Dtos;
using ASP_Library.Services;

namespace ASP_Library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT access token.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _authService.LoginAsync(request);
        if (response == null)
        {
            return Unauthorized(new { message = "Invalid username/email or password." });
        }

        return Ok(response);
    }
}
