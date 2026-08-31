using ASP_Library.Dtos;

namespace ASP_Library.Repositories;

public interface IUserRepository
{
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<int> CreateUserAsync(UserCreateDto user);
    Task<bool> UpdateUserAsync(int id, UserUpdateDto user);
    Task<bool> DeleteUserAsync(int id);
}
