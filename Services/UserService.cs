using ASP_Library.Dtos;
using ASP_Library.Repositories;

namespace ASP_Library.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        return _userRepository.GetAllUsersAsync();
    }

    public Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        return _userRepository.GetUserByIdAsync(id);
    }

    public Task<int> CreateUserAsync(UserCreateDto user)
    {
        return _userRepository.CreateUserAsync(user);
    }

    public Task<bool> UpdateUserAsync(int id, UserUpdateDto user)
    {
        return _userRepository.UpdateUserAsync(id, user);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        return _userRepository.DeleteUserAsync(id);
    }
}
