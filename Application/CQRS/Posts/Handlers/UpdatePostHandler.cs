using Application.Common.Interfaces;
using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Posts.Handlers
{
    public class UpdatePostHandler
    {
        public class Command : IRequest<ResponseModel<UpdatePostDto>>
        {
            public Command()
            {
            }

            public UpdatePostDto PostDto { get; set; }
        }

        public sealed class Handler : IRequestHandler<Command, ResponseModel<UpdatePostDto>>
        {
            private readonly IUnitOfWork _unitWork;
            private readonly IMapper _mapper;
            private readonly IUserContext _userContext;
            private readonly IActivityLoggerService _activityLogger;

            public Handler(IUnitOfWork unitWork, IMapper mapper, IActivityLoggerService activityLogger, IUserContext userContext)
            {
                _unitWork = unitWork;
                _mapper = mapper;
                _userContext = userContext;
                _activityLogger = activityLogger;
            }

            public async Task<ResponseModel<UpdatePostDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var post = await _unitWork.PostRepository.GetByIdAsync(request.PostDto.Id);

                if (post == null)
                    throw new NotFoundException("Post does not exist with the provided ID.");

                _mapper.Map(request.PostDto, post);
                post.UpdatedAt = DateTime.UtcNow;

                await _unitWork.PostRepository.UpdateAsync(post);
                await _unitWork.SaveChangesAsync();

                #region ActivityLog
                var currentUserId = _userContext.GetCurrentUserId();

                await _activityLogger.LogAsync(
                    userId: currentUserId.Value,
                    action: "Update",
                    entityType: "Post",
                    entityId: post.Id,
                    performedBy: currentUserId,
                    description: $"User {currentUserId} updated {post.Title} posts."
                );
                #endregion

                var response = _mapper.Map<UpdatePostDto>(post);

                return new ResponseModel<UpdatePostDto>
                {
                    Data = response,
                    IsSuccess = true
                };
            }
        }
    }
}
