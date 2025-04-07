using Application.Common.Interfaces;
using Application.CQRS.Posts.DTOs;
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

namespace Application.CQRS.Posts.Handlers;

public class DeletePostHandler
{
    public record Command (int Id , int DeletedByUserId , string DeletedReason) : IRequest<ResponseModel<string>>
    {

    }


    public sealed class Handler(IUnitOfWork unitWork, IUserContext userContext, IMapper mapper, IActivityLoggerService activityLogger) : IRequestHandler<Command, ResponseModel<string>>
    {
        private readonly IUnitOfWork _unitWork = unitWork;
        private readonly IUserContext _userContext = userContext;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;


        async Task<ResponseModel<string>> IRequestHandler<Command, ResponseModel<string>>.Handle(Command request, CancellationToken cancellationToken)
        {

            var currentUserId = _userContext.GetCurrentUserId();
            if (!currentUserId.HasValue) throw new UnauthorizedAccessException("User is not authenticated.");

            var userRole = _userContext.GetUserRole();
            if (userRole != "Admin") throw new ForbiddenException("Only Admins can delete posts.");

            if (string.IsNullOrWhiteSpace(request.DeletedReason)) throw new BadRequestException("Delete reason is required.");

            var post = await _unitWork.PostRepository.GetByIdAsync(request.Id);
            if (post == null) throw new BadRequestException("Post does not exist with the provided ID.");

            //command i posta ceviririk 
            _mapper.Map(request, post);
            post.DeletedBy = request.DeletedByUserId;


            await _unitWork.PostRepository.DeleteAsync(post);
            await _unitWork.SaveChangesAsync();

            #region ActivityLog


            await _activityLogger.LogAsync(

                    userId: currentUserId.Value,
                    action: "Delete",
                    entityType: "Post",
                    entityId: post.Id,
                    performedBy: currentUserId.Value,
                    description: $"Post with ID {post.Id} and Title '{post.Title}' was deleted by user {currentUserId.Value}. Reason: {request.DeletedReason}"
            );

            #endregion

            return new ResponseModel<string>
            {
                Data  = "Post deleted successfully",
                IsSuccess = true
            };
        }

       
    }


}

