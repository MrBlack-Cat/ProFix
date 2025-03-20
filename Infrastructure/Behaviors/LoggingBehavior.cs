﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[LoggingBehavior] Handling {typeof(TRequest).Name}");
        var response = await next();
        _logger.LogInformation($"[LoggingBehavior] Handled {typeof(TRequest).Name}");
        return response;
    }
}
