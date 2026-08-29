using ASP_Library.Dtos;
using ASP_Library.Entities;
using ASP_Library.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_Library.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AuthorResponseDto>> GetAllAuthorsAsync()
    {
        return await _context.Authors
            .Select(a => new AuthorResponseDto
            {
                AuthorId = a.AuthorId,
                FirstName = a.FirstName,
                LastName = a.LastName
            })
            .ToListAsync();
    }

    public async Task<AuthorResponseDto> GetAuthorByIdAsync(int authorId)
    {
        return await _context.Authors
            .Where(a => a.AuthorId == authorId)
            .Select(a => new AuthorResponseDto
            {
                AuthorId = a.AuthorId,
                FirstName = a.FirstName,
                LastName = a.LastName
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAuthorAsync(AuthorCreateDto author)
    {
        var authorEntity = new Author
        {
            FirstName = author.FirstName,
            LastName = author.LastName
        };

        _context.Authors.Add(authorEntity);
        await _context.SaveChangesAsync();
        return authorEntity.AuthorId;
    }

    public async Task<bool> UpdateAuthorAsync(int authorId, AuthorUpdateDto author)
    {
        var authorEntity = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);
        if (authorEntity == null)
        {
            return false;
        }

        authorEntity.FirstName = author.FirstName;
        authorEntity.LastName = author.LastName;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAuthorAsync(int authorId)
    {
        var authorEntity = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);
        if (authorEntity == null)
        {
            return false;
        }

        _context.Authors.Remove(authorEntity);
        await _context.SaveChangesAsync();
        return true;
    }
}