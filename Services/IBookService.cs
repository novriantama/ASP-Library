using ASP_Library.Dtos;

namespace ASP_Library.Services;

public interface IBookService
{
    Task<List<BookResponseDto>> GetAllBooksAsync();
    Task<BookResponseDto?> GetBookByIdAsync(int bookId);
    Task<int> CreateBookAsync(BookCreateDto book);
    Task<bool> UpdateBookAsync(int bookId, BookUpdateDto book);
    Task<bool> DeleteBookAsync(int bookId);
}
