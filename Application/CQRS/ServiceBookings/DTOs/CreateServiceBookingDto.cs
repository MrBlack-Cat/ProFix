namespace Application.CQRS.ServiceBookings.DTOs;

public class CreateServiceBookingDto
{
    public int ClientProfileId { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public string? Description { get; set; }
    public int StatusId { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public int? CreatedBy { get; set; }
}
