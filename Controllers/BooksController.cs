using Microsoft.AspNetCore.Mvc;
using ASP_Library.Dtos;
using ASP_Library.Services;

namespace ASP_Library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookResponseDto>>> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponseDto>> GetBookById(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound(new { message = $"Book with ID {id} not found." });
        }
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult> CreateBook([FromBody] BookCreateDto book)
    {
        var bookId = await _bookService.CreateBookAsync(book);
        return CreatedAtAction(nameof(GetBookById), new { id = bookId }, new { bookId, book.Title });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBook(int id, [FromBody] BookUpdateDto book)
    {
        var success = await _bookService.UpdateBookAsync(id, book);
        if (!success)
        {
            return NotFound(new { message = $"Book with ID {id} not found." });
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        var success = await _bookService.DeleteBookAsync(id);
        if (!success)
        {
            return NotFound(new { message = $"Book with ID {id} not found." });
        }
        return NoContent();
    }
}
