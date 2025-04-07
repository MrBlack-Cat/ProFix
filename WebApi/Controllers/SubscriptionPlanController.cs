using Application.CQRS.SubscriptionPlans.Commands.Requests;
using Application.CQRS.SubscriptionPlans.DTOs;
using Application.CQRS.SubscriptionPlans.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {

        private readonly IMediator _mediator;


        public SubscriptionPlanController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ResponseModel<CreateSubscriptionPlanDto>>> CreateSubscriptionPlan([FromBody] CreateSubscriptionPlanDto subscriptionPlanDto)
        {
            if (subscriptionPlanDto == null)
                return BadRequest(new ResponseModel<CreateSubscriptionPlanDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid subscription plan data."]
                });

            var command = new CreateSubscriptionPlanRequest(subscriptionPlanDto);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<DeleteSubscriptionPlanDto>>> DeleteSubscriptionPlan(int id, [FromBody] DeleteSubscriptionPlanDto deleteDto)
        {
            if (deleteDto == null || deleteDto.Id != id)
                return BadRequest(new ResponseModel<DeleteSubscriptionPlanDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid data or mismatched Id."]
                });

            var command = new DeleteSubscriptionPlanRequest(deleteDto);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpPut("Update{id}")]
        public async Task<ActionResult<ResponseModel<UpdateSubscriptionPlanDto>>> UpdateSubscriptionPlan(int id, [FromBody] UpdateSubscriptionPlanDto updateDto)
        {
            if (updateDto == null || updateDto.Id != id)
                return BadRequest(new ResponseModel<UpdateSubscriptionPlanDto>
                {
                    IsSuccess = false,
                    Errors =[ "Invalid data or mismatched Id."]
                });

            var command = new UpdateSubscriptionPlanRequest(updateDto);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetById{id}")]
        public async Task<ActionResult<ResponseModel<GetSubscriptionPlanByIdDto>>> GetSubscriptionPlanById(int id)
        {
            var query = new GetSubscriptionPlanByIdRequest(id);
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }


        [HttpGet("GetList")]
        public async Task<ActionResult<ResponseModel<List<SubscriptionPlanListDto>>>> GetListSubscriptionPlan()
        {
            var query = new GetListSubscriptionPlanRequest();
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
