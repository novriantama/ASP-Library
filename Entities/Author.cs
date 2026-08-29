namespace ASP_Library.Entities;

public class Author
{
    public int AuthorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
