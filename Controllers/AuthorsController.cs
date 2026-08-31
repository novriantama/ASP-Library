using Microsoft.AspNetCore.Mvc;
using ASP_Library.Dtos;
using ASP_Library.Services;
using Microsoft.AspNetCore.Authorization;

namespace ASP_Library.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuthorResponseDto>>> GetAllAuthors()
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorResponseDto>> GetAuthorById(int id)
    {
        var author = await _authorService.GetAuthorByIdAsync(id);
        if (author == null)
        {
            return NotFound(new { message = $"Author with ID {id} not found." });
        }
        return Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAuthor([FromBody] AuthorCreateDto author)
    {
        var authorId = await _authorService.CreateAuthorAsync(author);
        return CreatedAtAction(nameof(GetAuthorById), new { id = authorId }, new { authorId, author.FirstName, author.LastName });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuthor(int id, [FromBody] AuthorUpdateDto author)
    {
        var success = await _authorService.UpdateAuthorAsync(id, author);
        if (!success)
        {
            return NotFound(new { message = $"Author with ID {id} not found." });
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
        var success = await _authorService.DeleteAuthorAsync(id);
        if (!success)
        {
            return NotFound(new { message = $"Author with ID {id} not found." });
        }
        return NoContent();
    }
}
