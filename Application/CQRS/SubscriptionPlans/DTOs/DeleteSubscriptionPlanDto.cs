﻿namespace Application.CQRS.SubscriptionPlans.DTOs;

public class DeleteSubscriptionPlanDto
{
    public int Id { get; set; }
    public string? DeletedByUserId { get; set; }
    public string? Reason { get; set; }
}
