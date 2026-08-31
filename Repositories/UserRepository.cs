using ASP_Library.Data;
using ASP_Library.Dtos;
using ASP_Library.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP_Library.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.UserId == id)
            .Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string identifier)
    {
        var normalized = identifier.Trim().ToLowerInvariant();
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == normalized || u.Email.ToLower() == normalized);
    }

    public async Task<int> CreateUserAsync(UserCreateDto user)
    {
        var passwordHash = user.Password.StartsWith("$2") 
            ? user.Password 
            : BCrypt.Net.BCrypt.HashPassword(user.Password);

        var userEntity = new User
        {
            Username = user.Username.Trim(),
            Email = user.Email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();
        return userEntity.UserId;
    }

    public async Task<bool> UpdateUserAsync(int id, UserUpdateDto user)
    {
        var userEntity = await _context.Users.FindAsync(id);
        if (userEntity == null)
        {
            return false;
        }

        userEntity.Username = user.Username.Trim();
        userEntity.Email = user.Email.Trim().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            userEntity.PasswordHash = user.Password.StartsWith("$2")
                ? user.Password
                : BCrypt.Net.BCrypt.HashPassword(user.Password);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var userEntity = await _context.Users.FindAsync(id);
        if (userEntity == null)
        {
            return false;
        }

        _context.Users.Remove(userEntity);
        await _context.SaveChangesAsync();
        return true;
    }
}
