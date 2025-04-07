using Application.Common.Interfaces;
using Application.CQRS.SupportTickets.Commands.Requests;
using Application.CQRS.SupportTickets.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Infrastructure.Services;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SupportTickets.Commands.Handlers;

public class DeleteSupportTicketHandler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext) : IRequestHandler<DeleteSupportTicketRequest, ResponseModel<DeleteSupportTicketDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;


    public async Task<ResponseModel<DeleteSupportTicketDto>> Handle(DeleteSupportTicketRequest request, CancellationToken cancellationToken)
    {
        var supportTicketEntity = await _unitOfWork.SupportTicketRepository.GetByIdAsync(request.SupportTicketDto.Id);
        if (supportTicketEntity == null)
            throw new NotFoundException("Support ticket not found.");

        supportTicketEntity.DeletedReason = request.SupportTicketDto.DeletedReason;
        supportTicketEntity.DeletedBy = request.SupportTicketDto.DeletedByUserId;




        // Dəstək biletini silirik
        await _unitOfWork.SupportTicketRepository.DeleteAsync(supportTicketEntity);
        await _unitOfWork.SaveChangesAsync();

        #region ActivityLog
        var currentUserId = _userContext.GetCurrentUserId();  

        await _activityLogger.LogAsync(
            userId: currentUserId.Value,  
            action: "Delete",  
            entityType: "SupportTicket",  
            entityId: supportTicketEntity.Id,
            performedBy: currentUserId,  
            description: $"User {currentUserId.Value} deleted the support ticket with ID: {supportTicketEntity.Id}. Reason: {supportTicketEntity.DeletedReason}"  
        );

        #endregion


        var responseDto = _mapper.Map<DeleteSupportTicketDto>(supportTicketEntity);

        return new ResponseModel<DeleteSupportTicketDto>
        {
            Data = responseDto,
            IsSuccess = true
        };
    }
}