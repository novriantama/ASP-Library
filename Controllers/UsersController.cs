using Microsoft.AspNetCore.Mvc;
using ASP_Library.Dtos;
using ASP_Library.Services;

namespace ASP_Library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] UserCreateDto user)
    {
        var userId = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { userId, user.Username, user.Email });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] UserUpdateDto user)
    {
        var success = await _userService.UpdateUserAsync(id, user);
        if (!success)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }
        return NoContent();
    }
}
