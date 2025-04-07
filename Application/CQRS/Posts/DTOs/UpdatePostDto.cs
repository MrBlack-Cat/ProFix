namespace Application.CQRS.Posts.DTOs;

public class UpdatePostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public int? UpdatedBy { get; set; }
}
