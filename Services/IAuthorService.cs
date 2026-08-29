using ASP_Library.Dtos;
using ASP_Library.Entities;

namespace ASP_Library.Services;

public interface IAuthorService
{
    Task<List<AuthorResponseDto>> GetAllAuthorsAsync();
    Task<AuthorResponseDto> GetAuthorByIdAsync(int authorId);
    Task<int> CreateAuthorAsync(AuthorCreateDto author);
    Task<bool> UpdateAuthorAsync(int authorId, AuthorUpdateDto author);
    Task<bool> DeleteAuthorAsync(int authorId);
}