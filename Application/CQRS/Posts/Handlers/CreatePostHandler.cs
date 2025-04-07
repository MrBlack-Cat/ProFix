using Application.Common.Interfaces;
using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;



namespace Application.CQRS.Posts.Handlers;

public class CreatePostHandler
{

    public record struct Command : IRequest<ResponseModel<CreatePostDto>>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
    }


    public sealed class Handler(IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper, IActivityLoggerService activityLogger) : IRequestHandler<Command, ResponseModel<CreatePostDto>>
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserContext _userContext = userContext;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;




        public async Task<ResponseModel<CreatePostDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var currenUserId = _userContext.GetCurrentUserId();
            if (!currenUserId.HasValue)
                throw new UnauthorizedAccessException("User is not authenticated.");

            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
                throw new BadRequestException("Title and content are required.");


            //burada Command i Post a ceviririk //database e elave elemek ucun 

            var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(currenUserId.Value);



            //create by context-den gelmelidi ona göre deyişdim.

            if (profile is null)
                throw new NotFoundException("Service provider profile not found.");

            var newPost = new Post
            {
                Title = request.Title,
                Content = request.Content,
                ImageUrl = request.ImageUrl,
                ServiceProviderProfileId = profile.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currenUserId.Value
            };
            await _unitOfWork.PostRepository.AddAsync(newPost);
            await _unitOfWork.SaveChangesAsync();


            #region ActivityLog

                await _activityLogger.LogAsync(


            userId: currenUserId.Value,
                    action: "Create",
                    entityType: "Post",
                    entityId: newPost.Id,
                    performedBy: currenUserId.Value,
                    description: $"Post with title '{newPost.Title}' was created by user {currenUserId.Value}."
                    

            );

         
            #endregion


            //burada ise post u dto ya ceviririk

            var responseDto = _mapper.Map<CreatePostDto>(newPost);

            return new ResponseModel<CreatePostDto>
            {
                Data = responseDto,
                IsSuccess = true
            };
        }
    }
}
