﻿namespace Application.CQRS.Notifications.DTOs;

public class NotificationListDto
{
    public int Id { get; set; }
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
