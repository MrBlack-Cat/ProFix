﻿using Application.CQRS.ActivityLogs.DTOs;
using Application.CQRS.ActivityLogs.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ActivityLogs.Queries.Handlers;

public class GetAllActivityLogsQueryHandler : IRequestHandler<GetAllActivityLogsQuery, ResponseModel<List<ActivityLogListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllActivityLogsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ActivityLogListDto>>> Handle(GetAllActivityLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _unitOfWork.ActivityLogRepository.GetAllAsync();
        var result = _mapper.Map<List<ActivityLogListDto>>(logs);

        return new ResponseModel<List<ActivityLogListDto>>
        {
            Data = result,
            IsSuccess = true
        };
    }
}
