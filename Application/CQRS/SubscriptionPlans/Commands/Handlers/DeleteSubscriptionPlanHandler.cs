using Application.Common.Interfaces;
using Application.CQRS.SubscriptionPlans.Commands.Requests;
using Application.CQRS.SubscriptionPlans.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SubscriptionPlans.Commands.Handlers;

public class DeleteSubscriptionPlanHandler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext ) : IRequestHandler<DeleteSubscriptionPlanRequest, ResponseModel<DeleteSubscriptionPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;

    public async Task<ResponseModel<DeleteSubscriptionPlanDto>> Handle(DeleteSubscriptionPlanRequest request, CancellationToken cancellationToken)
    {
        var subscriptionPlanEntity = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(request.SubscriptionPlanDto.Id);
        if (subscriptionPlanEntity == null)
        {
            throw new NotFoundException("SubscriptionPlan not found.");
        }

        subscriptionPlanEntity.DeletedBy = request.SubscriptionPlanDto.DeletedByUserId;
        subscriptionPlanEntity.DeletedReason = request.SubscriptionPlanDto.Reason;
        subscriptionPlanEntity.DeletedAt = DateTime.UtcNow;

        await _unitOfWork.SubscriptionPlanRepository.DeleteAsync(subscriptionPlanEntity);
        await _unitOfWork.SaveChangesAsync();

        var currentUserId = _userContext.GetCurrentUserId();
        var currentUserName = _userContext.GetCurrentUserName();

        await _activityLogger.LogAsync(
                userId: currentUserId.Value,
                action: "Delete",
                entityType: "SubscriptionPlan",
                entityId: subscriptionPlanEntity.Id,
                performedBy: currentUserId.Value,
                description: $"User {currentUserName} (ID: {currentUserId}) deleted Subscription Plan: {subscriptionPlanEntity.PlanName} (ID: {subscriptionPlanEntity.Id}). Reason: {subscriptionPlanEntity.DeletedReason}"
            );

        var responseDto = _mapper.Map<DeleteSubscriptionPlanDto>(subscriptionPlanEntity);

        return new ResponseModel<DeleteSubscriptionPlanDto>
        {
            Data = responseDto,
            IsSuccess = true
        };
    }
}
