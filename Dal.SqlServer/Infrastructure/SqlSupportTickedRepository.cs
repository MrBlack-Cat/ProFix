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
    public class SqlSupportTicketRepository : ISupportTicketRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlSupportTicketRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<SupportTicket>> GetAllAsync()
        {
            var sql = "SELECT * FROM SupportTicket WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<SupportTicket>(sql);
        }

        public async Task<SupportTicket?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM SupportTicket WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<SupportTicket>(sql, new { Id = id });
        }

        public async Task<IEnumerable<SupportTicket>> GetByUserIdAsync(int userId)
        {
            var sql = "SELECT * FROM SupportTicket WHERE UserId = @UserId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<SupportTicket>(sql, new { UserId = userId });
        }

        public async Task AddAsync(SupportTicket entity)
        {
            var sql = @"
                INSERT INTO SupportTicket (UserId, Subject, Message, StatusId, CreatedAt, CreatedBy)
                VALUES (@UserId, @Subject, @Message, @StatusId, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(SupportTicket entity)
        {
            var sql = @"
                UPDATE SupportTicket SET
                    Subject = @Subject,
                    Message = @Message,
                    StatusId = @StatusId,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(SupportTicket entity)
        {
            var sql = @"
                UPDATE SupportTicket SET
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
