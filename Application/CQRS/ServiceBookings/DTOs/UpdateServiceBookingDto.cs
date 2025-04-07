namespace Application.CQRS.ServiceBookings.DTOs;

public class UpdateServiceBookingDto
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public int StatusId { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public int? UpdatedBy { get; set; }
}
