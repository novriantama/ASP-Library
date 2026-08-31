using ASP_Library.Dtos;
using ASP_Library.Entities;

namespace ASP_Library.Repositories;

public interface IUserRepository
{
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<User?> GetByUsernameOrEmailAsync(string identifier);
    Task<int> CreateUserAsync(UserCreateDto user);
    Task<bool> UpdateUserAsync(int id, UserUpdateDto user);
    Task<bool> DeleteUserAsync(int id);
}
