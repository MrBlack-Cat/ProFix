namespace Application.CQRS.SupportTickets.DTOs;

public class DeleteSupportTicketDto
{
    public int Id { get; set; }
    public int? DeletedByUserId { get; set; }
    public string? DeletedReason { get; set; }
    
}
