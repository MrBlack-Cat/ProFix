namespace Application.CQRS.Reviews.DTOs;

public class CreateReviewDto
{
    public int ClientProfileId { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public int Rating { get; set; } // 1–5
    public string? Comment { get; set; }
    public int? CreatedBy { get; set; }
}
