namespace Application.CQRS.Reviews.DTOs;

public class UpdateReviewDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public int? UpdatedBy { get; set; }
}
