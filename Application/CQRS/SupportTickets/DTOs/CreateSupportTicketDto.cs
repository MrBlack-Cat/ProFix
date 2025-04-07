namespace Application.CQRS.SupportTickets.DTOs;

public class CreateSupportTicketDto
{
    public int UserId { get; set; }
    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;
    public int StatusId { get; set; }
    public int? CreatedBy { get; set; }
}
