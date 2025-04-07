using Application.CQRS.SubscriptionPlans.DTOs;
using Application.CQRS.SupportTickets.Commands.Requests;
using Application.CQRS.SupportTickets.DTOs;
using Application.CQRS.SupportTickets.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportTicketController : ControllerBase
    {

        private readonly IMediator _mediator;


        public SupportTicketController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ResponseModel<CreateSupportTicketDto>>> CreateSupportTicket([FromBody] CreateSupportTicketDto supportTicketDto)
        {

            if (supportTicketDto == null)
                return BadRequest(new ResponseModel<CreateSupportTicketDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid support ticket data."]
                });

            var command = new CreateSupportTicketRequest(supportTicketDto);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<DeleteSupportTicketDto>>> DeleteSupportTicket(int id, [FromBody] DeleteSupportTicketDto supportTicketDto)
        {
            if (id != supportTicketDto.Id)
                return BadRequest("Ticket ID mismatch");

            var command = new DeleteSupportTicketRequest(supportTicketDto);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("Update{id}")]
        public async Task<ActionResult<ResponseModel<UpdateSupportTicketDto>>> UpdateSupportTicket(int id, [FromBody] UpdateSupportTicketDto supportTicketDto)
        {
            if (id != supportTicketDto.Id)
                return BadRequest("Ticket ID mismatch");

            var command = new UpdateSupportTicketRequest(supportTicketDto);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }



        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseModel<GetSupportTicketByIdDto>>> GetSupportTicketById(int id)
        {
            var query = new GetSupportTicketByIdRequest(id);
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet("GetList")]
        public async Task<ActionResult<ResponseModel<List<SupportTicketListDto>>>> GetSupportTicketList()
        {
            var query = new SupportTicketListRequest();
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

    }



}

