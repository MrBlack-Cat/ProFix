using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

    public interface IActivityLoggerService
    {
        // General
        Task LogAsync(
            int userId,
            string action,
            string entityType,
            int entityId,
        List<int>? entityIds = null, // yeni elave eledim GetList zamani hamsini ekrana almaq ucun 
            int ? DeletedBy = null,
            int? performedBy = null,
            string? description = null);

        Task LogAsync(
            int userId,
            string action,
            string entityType,
            int? performedBy = null,
            string? description = null);

        // System
        Task LogAsync(
            string action,
            string entityType,
            string? description = null);
    }

