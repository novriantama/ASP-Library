using System.ComponentModel.DataAnnotations;

namespace ASP_Library.Dtos;

public class BookCreateDto
{
    [Required(ErrorMessage = "ISBN is required")]
    [StringLength(13, ErrorMessage = "ISBN cannot exceed 13 characters")]
    public string Isbn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Title is required")]
    [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Publish Date is required")]
    [DataType(DataType.Date)]
    public DateOnly PublishDate { get; set; }

    [Required(ErrorMessage = "Author is required")]
    [MinLength(1, ErrorMessage = "At least one author is required")]
    public List<int> AuthorIds { get; set; } = new List<int>();
}