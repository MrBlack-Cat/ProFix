using Application.Common.Interfaces;
using Application.CQRS.ServiceBookings.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ServiceBookings.Handler;

public class DeleteServiceBookingHandler
{

    public record struct DeleteServiceBookingCommand(int Id, int? DeletedByUserId, string? Reason) : IRequest<ResponseModel<DeleteServiceBookingDto>>;


    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger)
    : IRequestHandler<DeleteServiceBookingCommand, ResponseModel<DeleteServiceBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;


        public async Task<ResponseModel<DeleteServiceBookingDto>> Handle(DeleteServiceBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.ServiceBookingRepository.GetByIdAsync(request.Id);
            if (booking == null)
                throw new NotFoundException("Service booking not found.");

            var deletedDto = new DeleteServiceBookingDto
            {
                Id = booking.Id,
                DeletedByUserId = request.DeletedByUserId,
                DeletedReason = request.Reason
            };

            await _unitOfWork.ServiceBookingRepository.DeleteAsync(booking);
            await _unitOfWork.SaveChangesAsync();


            #region ActivityLog
            await _activityLogger.LogAsync(
                userId: request.DeletedByUserId.Value,  
                action: "Delete",                  
                entityType: "ServiceBooking",      
                entityId: booking.Id,             
                performedBy: request.DeletedByUserId,
                description: $"Service booking for ClientProfileId {booking.ClientProfileId} and ServiceProviderProfileId {booking.ServiceProviderProfileId} deleted. " +
                             $"Scheduled Date: {booking.ScheduledDate}. Reason: {request.Reason}."
            );
            #endregion



            return new ResponseModel<DeleteServiceBookingDto>
            {
                Data = deletedDto,
                IsSuccess = true,
                Errors = [],
            };
        }
    }




}
