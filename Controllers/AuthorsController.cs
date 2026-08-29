using Microsoft.AspNetCore.Mvc;
using ASP_Library.Services;

namespace ASP_Library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;

        [HttpGet("")]
        public async Task<ActionResult<List<Author>>> GetAllAuthors() {
            var authors = await _authorService.GetAllAuthorsAsync();
            if (authors == null) {
                return NotFound();
            }
            return Ok(authors);
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<Author>> GetAuthorById(int id) {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null) {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateAuthor([FromBody] AuthorCreateDto author) {
            var authorId = await _authorService.CreateAuthorAsync(author);
            return CreatedAtAction(nameof(GetAuthorById), new { id = authorId }, author);
        }

        [HttpPut("/{id}")]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] AuthorUpdateDto author) {
            var success = await _authorService.UpdateAuthorAsync(id, author);
            if (!success) {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/{id}")]
        public async Task<ActionResult> DeleteAuthor(int id) {
            var success = await _authorService.DeleteAuthorAsync(id);
            if (!success) {
                return NotFound();
            }
            return NoContent();
        }
    }
}
