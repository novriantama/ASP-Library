using ASP_Library.Data;
using ASP_Library.Dtos;
using ASP_Library.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP_Library.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookResponseDto>> GetAllBooksAsync()
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .Select(b => new BookResponseDto
            {
                BookId = b.BookId,
                Isbn = b.Isbn,
                Title = b.Title,
                PublishDate = b.PublishDate ?? default,
                Authors = b.BookAuthors.Select(ba => new AuthorResponseDto
                {
                    AuthorId = ba.Author.AuthorId,
                    FirstName = ba.Author.FirstName,
                    LastName = ba.Author.LastName
                }).ToList()
            }).ToListAsync();
    }

    public async Task<BookResponseDto?> GetBookByIdAsync(int id)
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .Where(b => b.BookId == id)
            .Select(b => new BookResponseDto
            {
                BookId = b.BookId,
                Isbn = b.Isbn,
                Title = b.Title,
                PublishDate = b.PublishDate ?? default,
                Authors = b.BookAuthors.Select(ba => new AuthorResponseDto
                {
                    AuthorId = ba.Author.AuthorId,
                    FirstName = ba.Author.FirstName,
                    LastName = ba.Author.LastName
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task<int> CreateBookAsync(BookCreateDto book)
    {
        var bookEntity = new Book
        {
            Isbn = book.Isbn,
            Title = book.Title,
            PublishDate = book.PublishDate
        };

        _context.Books.Add(bookEntity);
        await _context.SaveChangesAsync();

        if (book.AuthorIds != null && book.AuthorIds.Count > 0)
        {
            foreach (var authorId in book.AuthorIds)
            {
                _context.BookAuthors.Add(new BookAuthor
                {
                    BookId = bookEntity.BookId,
                    AuthorId = authorId
                });
            }
            await _context.SaveChangesAsync();
        }

        return bookEntity.BookId;
    }

    public async Task<bool> UpdateBookAsync(int id, BookUpdateDto book)
    {
        var bookEntity = await _context.Books.FindAsync(id);
        if (bookEntity == null)
        {
            return false;
        }

        bookEntity.Isbn = book.Isbn;
        bookEntity.Title = book.Title;
        bookEntity.PublishDate = book.PublishDate;

        // Update authors
        if (book.AuthorIds != null)
        {
            var existingAuthors = _context.BookAuthors
                .Where(ba => ba.BookId == id)
                .ToList();

            var newAuthorIds = new HashSet<int>(book.AuthorIds);

            foreach (var existing in existingAuthors)
            {
                if (!newAuthorIds.Contains(existing.AuthorId))
                {
                    _context.BookAuthors.Remove(existing);
                }
                else
                {
                    newAuthorIds.Remove(existing.AuthorId);
                }
            }

            foreach (var newAuthorId in newAuthorIds)
            {
                _context.BookAuthors.Add(new BookAuthor
                {
                    BookId = id,
                    AuthorId = newAuthorId
                });
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var bookEntity = await _context.Books.FindAsync(id);
        if (bookEntity == null)
        {
            return false;
        }

        _context.Books.Remove(bookEntity);
        await _context.SaveChangesAsync();
        return true;
    }
}
