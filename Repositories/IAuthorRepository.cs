using ASP_Library.Dtos;

namespace ASP_Library.Repositories;

public interface IAuthorRepository
{
    Task<List<AuthorResponseDto>> GetAllAuthorsAsync();
    Task<AuthorResponseDto?> GetAuthorByIdAsync(int authorId);
    Task<int> CreateAuthorAsync(AuthorCreateDto author);
    Task<bool> UpdateAuthorAsync(int authorId, AuthorUpdateDto author);
    Task<bool> DeleteAuthorAsync(int authorId);
}