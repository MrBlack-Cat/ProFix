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
    public class SqlReviewRepository : IReviewRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlReviewRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            var sql = "SELECT * FROM Review WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<Review>(sql);
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Review WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<Review>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Review>> GetByServiceProviderIdAsync(int serviceProviderProfileId)
        {
            var sql = "SELECT * FROM Review WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<Review>(sql, new { ServiceProviderProfileId = serviceProviderProfileId });
        }

        public async Task<IEnumerable<Review>> GetByClientIdAsync(int clientProfileId)
        {
            var sql = "SELECT * FROM Review WHERE ClientProfileId = @ClientProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<Review>(sql, new { ClientProfileId = clientProfileId });
        }

        public async Task AddAsync(Review entity)
        {
            var sql = @"
                INSERT INTO Review (ClientProfileId, ServiceProviderProfileId, Rating, Comment, CreatedAt, CreatedBy)
                VALUES (@ClientProfileId, @ServiceProviderProfileId, @Rating, @Comment, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(Review entity)
        {
            var sql = @"
                UPDATE Review SET
                    Rating = @Rating,
                    Comment = @Comment,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(Review entity)
        {
            var sql = @"
                UPDATE Review SET 
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
