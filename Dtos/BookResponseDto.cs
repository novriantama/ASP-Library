namespace ASP_Library.Dtos;

public class BookResponseDto
{
    public int BookId { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateOnly PublishDate { get; set; }

    public List<AuthorResponseDto> Authors { get; set; } = [];
}