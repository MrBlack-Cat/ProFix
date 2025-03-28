﻿using Application.Common.Interfaces;
using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Posts.Handlers;

public class CreatePostHandler
{

    public record struct Command : IRequest<ResponseModel<CreatePostDto>>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int CreatedBy { get; set; }
    }


    public sealed class Handler(IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper) : IRequestHandler<Command, ResponseModel<CreatePostDto>>
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserContext _userContext = userContext;
        private readonly IMapper _mapper = mapper;



        public async Task<ResponseModel<CreatePostDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var currenUserId = _userContext.GetCurrentUserId();
            if (!currenUserId.HasValue)
                throw new UnauthorizedAccessException("User is not authenticated.");

            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
                throw new BadRequestException("Title and content are required.");


            //burada Command i Post a ceviririk //database e elave elemek ucun 

            var newPost = _mapper.Map<Post>(request);
            newPost.CreatedBy = request.CreatedBy;

            await _unitOfWork.PostRepository.AddAsync(newPost);
            await _unitOfWork.SaveChangesAsync();
            
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
