namespace ASP_Library.Entities;

public class Book
{
    public int BookId { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateOnly? PublishDate { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
