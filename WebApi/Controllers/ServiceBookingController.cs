using Application.CQRS.ServiceBookings.DTOs;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.ServiceBookings.Handler.CreateServiceBookingHandler;
using static Application.CQRS.ServiceBookings.Handler.DeleteServiceBookingHandler;
using static Application.CQRS.ServiceBookings.Handler.GetServiceBookingByIdHandler;
using static Application.CQRS.ServiceBookings.Handler.ServiceBookingListHandler;
using static Application.CQRS.ServiceBookings.Handler.UpdateServiceBookingHandler;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceBookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceBookingController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }



        [HttpPost]
        public async Task<ActionResult<ResponseModel<CreateServiceBookingDto>>> Create([FromBody] CreateServiceBookingDto dto)
        {

            if (dto == null)
                return BadRequest(new ResponseModel<CreateServiceBookingDto>
                {
                    IsSuccess = false,
                    Errors = ["Request body cannot be null."]
                });



            if (dto.ClientProfileId <= 0 || dto.ServiceProviderProfileId <= 0 || dto.StatusId <= 0)
                return BadRequest(new ResponseModel<CreateServiceBookingDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid data."]
                });

            var command = new CreateServiceBookingCommand(dto);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<DeleteServiceBookingDto>>> Delete(int id, [FromQuery] int? deletedByUserId, [FromQuery] string? reason)
        {
            if (id <= 0)
                return BadRequest(new ResponseModel<DeleteServiceBookingDto>
                {
                    IsSuccess = false,
                    Errors =[ "Invalid ID."]
                });

            var command = new DeleteServiceBookingCommand(id, deletedByUserId, reason);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }


        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseModel<GetServiceBookingByIdDto>>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new ResponseModel<GetServiceBookingByIdDto>
                {
                    IsSuccess = false,
                    Errors =[ "Invalid ID."]
                });

            var query = new GetServiceBookingByIdQuery(id);
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }



        [HttpGet("ListOfBookings")]
        public async Task<ActionResult<ResponseModel<List<ServiceBookingListDto>>>> GetAll()
        {
            var query = new ServiceBookingListQuery();
            var result = await _mediator.Send(query);


            if (result == null)
            {
                throw new NullReferenceException("Query result is null. Check MediatR handler implementation.");
            }


            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<ResponseModel<UpdateServiceBookingDto>>> Update(UpdateServiceBookingDto serviceBookingDto)
        {
            if (serviceBookingDto.Id <= 0)
                return BadRequest(new ResponseModel<UpdateServiceBookingDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid ID."]
                });

            var command = new UpdateServiceBookingCommand(serviceBookingDto);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
