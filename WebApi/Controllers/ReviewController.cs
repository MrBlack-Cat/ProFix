using Application.CQRS.Reviews.DTOs;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Reviews.Handlers.CreateReviewHandler;
using static Application.CQRS.Reviews.Handlers.DeleteReviewHandler;
using static Application.CQRS.Reviews.Handlers.GetReviewByIdHandler;
using static Application.CQRS.Reviews.Handlers.ReviewListHandler;
using static Application.CQRS.Reviews.Handlers.UpdateReviewHandler;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ResponseModel<CreateReviewDto>>> Create([FromBody] CreateReviewDto dto)
        {
            if (dto == null || dto.ClientProfileId <= 0 || dto.ServiceProviderProfileId <= 0)
                return BadRequest(new ResponseModel<CreateReviewDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid data."]
                });

            var command = new CreateReviewCommand(dto);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<DeleteReviewDto>>> Delete(int id, [FromQuery] int? deletedByUserId, [FromQuery] string? reason)
        {
            if (id <= 0)
                return BadRequest(new ResponseModel<DeleteReviewDto>
                {
                    IsSuccess = false,
                    Errors =[ "Invalid ID."]
                });

            var command = new DeleteReviewCommand(id, deletedByUserId, reason);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }


        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseModel<GetReviewByIdDto>>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new ResponseModel<GetReviewByIdDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid ID."]
                });

            var query = new GetReviewByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("ListOfReviews")]
        public async Task<ActionResult<ResponseModel<List<ReviewListDto>>>> GetAll()
        {
            var query = new GetReviewListQuery();
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }


        [HttpPut("Update")]
        public async Task<ActionResult<ResponseModel<UpdateReviewDto>>> Update([FromBody] UpdateReviewDto dto)
        {
            if (dto.Id <= 0 || dto.Rating < 1 || dto.Rating > 5)
                return BadRequest(new ResponseModel<UpdateReviewDto>
                {
                    IsSuccess = false,
                    Errors =[ "Invalid data."]
                });

            var command = new UpdateReviewCommand(dto);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
