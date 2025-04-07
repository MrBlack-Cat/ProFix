using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dal.SqlServer.Infrastructure
{
    public class SqlSubscriptionPlanRepository : ISubscriptionPlanRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlSubscriptionPlanRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<SubscriptionPlan>> GetAllAsync()
        {
            var sql = "SELECT * FROM SubscriptionPlan WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<SubscriptionPlan>(sql);
        }

        public async Task<SubscriptionPlan?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM SubscriptionPlan WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<SubscriptionPlan>(sql, new { Id = id });
        }

        public async Task<IEnumerable<SubscriptionPlan>> GetByServiceProviderIdAsync(int serviceProviderProfileId)
        {
            var sql = "SELECT * FROM SubscriptionPlan WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<SubscriptionPlan>(sql, new { ServiceProviderProfileId = serviceProviderProfileId });
        }

        public async Task AddAsync(SubscriptionPlan entity)
        {
            var sql = @"
                INSERT INTO SubscriptionPlan (ServiceProviderProfileId, PlanName, Price, DurationInDays, StartDate, EndDate, CreatedAt, CreatedBy)
                VALUES (@ServiceProviderProfileId, @PlanName, @Price, @DurationInDays, @StartDate, @EndDate, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(SubscriptionPlan entity)
        {
            var sql = @"
                UPDATE SubscriptionPlan SET
                    PlanName = @PlanName,
                    Price = @Price,
                    DurationInDays = @DurationInDays,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(SubscriptionPlan entity)
        {
            var sql = @"
                UPDATE SubscriptionPlan SET
                    IsDeleted = 1,
                    DeletedAt = @DeletedAt,
                    DeletedBy = @DeletedBy,
                    DeletedReason = @DeletedReason
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new
            {
                entity.Id,
                DeletedAt = entity.DeletedAt ?? DateTime.UtcNow,
                entity.DeletedBy,
                entity.DeletedReason
            });
        }

        public async Task DeleteAsync(int id , ClaimsPrincipal user)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            int userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            entity.DeletedBy = userId;
            entity.DeletedReason = "Soft delete by Id";

            await DeleteAsync(entity);
        }
    }
}
