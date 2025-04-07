using Application.CQRS.SupportTickets.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SupportTickets.Commands.Responses;

public class CreateSupportTicketResponse : ResponseModel<CreateSupportTicketDto>
{
    public CreateSupportTicketResponse(CreateSupportTicketDto data)
    {
        Data = data;
        IsSuccess = true;
    }
}
