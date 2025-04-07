namespace Application.CQRS.ServiceBookings.DTOs;

public class DeleteServiceBookingDto
{
    public int Id { get; set; }
    public int? DeletedByUserId { get; set; }
    public string? DeletedReason { get; set; }
}
