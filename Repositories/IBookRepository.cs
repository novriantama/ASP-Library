using ASP_Library.Dtos;

namespace ASP_Library.Repositories;

public interface IBookRepository
{
    Task<List<BookResponseDto>> GetAllBooksAsync();
    Task<BookResponseDto?> GetBookByIdAsync(int id);
    Task<int> CreateBookAsync(BookCreateDto book);
    Task<bool> UpdateBookAsync(int id, BookUpdateDto book);
    Task<bool> DeleteBookAsync(int id);
}
