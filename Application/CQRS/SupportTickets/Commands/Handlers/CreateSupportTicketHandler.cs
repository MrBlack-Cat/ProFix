using Application.Common.Interfaces;
using Application.CQRS.SupportTickets.Commands.Requests;
using Application.CQRS.SupportTickets.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SupportTickets.Commands.Handlers;

public class CreateSupportTicketHandler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext) : IRequestHandler<CreateSupportTicketRequest, ResponseModel<CreateSupportTicketDto>>
{



    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;

    public async Task<ResponseModel<CreateSupportTicketDto>> Handle(CreateSupportTicketRequest request, CancellationToken cancellationToken)
    {

        if (request.SupportTicketDto == null)
        {
            throw new ArgumentNullException(nameof(request.SupportTicketDto), "Support ticket DTO cannot be null.");
        }

        var supportTicketEntity = _mapper.Map<SupportTicket>(request.SupportTicketDto);
        await _unitOfWork.SupportTicketRepository.AddAsync(supportTicketEntity);
        await _unitOfWork.SaveChangesAsync();

        #region ActivityLog

        var currentUserId = _userContext.GetCurrentUserId();  

        await _activityLogger.LogAsync(
            userId: currentUserId.Value,  
            action: "Create",  
            entityType: "SupportTicket",  
            DeletedBy: currentUserId.Value,
            entityId: supportTicketEntity.Id,
            performedBy: currentUserId,  
            description: $"User {currentUserId.Value} created a new support ticket with ID: {supportTicketEntity.Id}."  
        );



        #endregion





        var responseDto = _mapper.Map<CreateSupportTicketDto>(supportTicketEntity);

        return new ResponseModel<CreateSupportTicketDto>
        {
            Data = responseDto,
            IsSuccess = true
        };
    }
}
