using ASP_Library.Dtos;

namespace ASP_Library.Services;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<int> CreateUserAsync(UserCreateDto user);
    Task<bool> UpdateUserAsync(int id, UserUpdateDto user);
    Task<bool> DeleteUserAsync(int id);
}
