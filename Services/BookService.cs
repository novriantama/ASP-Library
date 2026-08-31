using ASP_Library.Dtos;
using ASP_Library.Repositories;

namespace ASP_Library.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookResponseDto>> GetAllBooksAsync()
    {
        return await _bookRepository.GetAllBooksAsync();
    }

    public async Task<BookResponseDto?> GetBookByIdAsync(int id)
    {
        return await _bookRepository.GetBookByIdAsync(id);
    }

    public async Task<int> CreateBookAsync(BookCreateDto book)
    {
        return await _bookRepository.CreateBookAsync(book);
    }

    public async Task<bool> UpdateBookAsync(int id, BookUpdateDto book)
    {
        return await _bookRepository.UpdateBookAsync(id, book);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        return await _bookRepository.DeleteBookAsync(id);
    }
}
