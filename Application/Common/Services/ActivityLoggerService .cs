using Application.Common.Interfaces;
using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Services;

public class ActivityLoggerService(IUnitOfWork unitOfWork, ILoggerService loggerService) : IActivityLoggerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILoggerService _loggerService = loggerService;
    
    #region LogAsync


    public async Task LogAsync(
        int userId,
        string action,
        string entityType,
        int entityId,
        List<int>? entityIds = null, //burada elave eledim 
        int? DeletedBy = null,
        int? performedBy = null,
        string? description = null)
    {
        var log = new ActivityLog
        {
            UserId = userId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = performedBy ?? userId
        };

        await SaveAndLog(log, description);
    }
    #endregion

    #region LogAsync1

    public async Task LogAsync(
        int userId,
        string action,
        string entityType,
        int? performedBy = null,
        string? description = null)
    {
        var log = new ActivityLog
        {
            UserId = userId,
            Action = action,
            EntityType = entityType,
            EntityId = 0,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = performedBy ?? userId
        };

        await SaveAndLog(log, description);
    }

    #endregion

    #region LogAsync2

    public async Task LogAsync(
        string action,
        string entityType,
        string? description = null)
    {
        var log = new ActivityLog
        {
            UserId = 0,
            Action = action,
            EntityType = entityType,
            EntityId = 0,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = null
        };

        await SaveAndLog(log, description);
    }
    
    #endregion

    #region SaveAndLog

    private async Task SaveAndLog(ActivityLog log, string? description)
    {
        try
        {
            await _unitOfWork.ActivityLogRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();

            var msg = $"[ActivityLog] {log.Action} | Entity: {log.EntityType} (ID: {log.EntityId}) | UserId: {log.UserId}";
            if (!string.IsNullOrWhiteSpace(description))
                msg += $" | Description: {description}";

            _loggerService.LogInfo(msg);
        }
        catch (Exception ex)
        {
            _loggerService.LogError("Error when logging activity", ex);
        }
    }
    #endregion
}