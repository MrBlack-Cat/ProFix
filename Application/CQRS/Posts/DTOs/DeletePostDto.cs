namespace Application.CQRS.Posts.DTOs;

public class DeletePostDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedReason { get; set; }
}
