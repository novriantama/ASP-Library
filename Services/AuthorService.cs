using ASP_Library.Dtos;
using ASP_Library.Repositories;

namespace ASP_Library.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public Task<List<AuthorResponseDto>> GetAllAuthorsAsync()
    {
        return _authorRepository.GetAllAuthorsAsync();
    }

    public Task<AuthorResponseDto?> GetAuthorByIdAsync(int authorId)
    {
        return _authorRepository.GetAuthorByIdAsync(authorId);
    }

    public Task<int> CreateAuthorAsync(AuthorCreateDto author)
    {
        return _authorRepository.CreateAuthorAsync(author);
    }

    public Task<bool> UpdateAuthorAsync(int authorId, AuthorUpdateDto author)
    {
        return _authorRepository.UpdateAuthorAsync(authorId, author);
    }

    public Task<bool> DeleteAuthorAsync(int authorId)
    {
        return _authorRepository.DeleteAuthorAsync(authorId);
    }
}