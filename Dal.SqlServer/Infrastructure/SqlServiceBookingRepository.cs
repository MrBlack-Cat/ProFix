﻿using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.SqlServer.Infrastructure
{
    public class SqlServiceBookingRepository : IServiceBookingRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlServiceBookingRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<ServiceBooking>> GetAllAsync()
        {
            var sql = "SELECT * FROM ServiceBookings WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<ServiceBooking>(sql);
        }

        public async Task<ServiceBooking?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM ServiceBookings WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<ServiceBooking>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ServiceBooking>> GetByClientIdAsync(int clientProfileId)
        {
            var sql = "SELECT * FROM ServiceBookings WHERE ClientProfileId = @ClientProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<ServiceBooking>(sql, new { ClientProfileId = clientProfileId });
        }

        public async Task<IEnumerable<ServiceBooking>> GetByServiceProviderIdAsync(int serviceProviderProfileId)
        {
            var sql = "SELECT * FROM ServiceBookings WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<ServiceBooking>(sql, new { ServiceProviderProfileId = serviceProviderProfileId });
        }

        public async Task AddAsync(ServiceBooking entity)
        {
            var sql = @"
                INSERT INTO ServiceBookings (ClientProfileId, ServiceProviderProfileId, Description, StatusId, ScheduledDate, CreatedAt, CreatedBy)
                VALUES (@ClientProfileId, @ServiceProviderProfileId, @Description, @StatusId, @ScheduledDate, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(ServiceBooking entity)
        {
            var sql = @"
                UPDATE ServiceBookings SET
                    Description = @Description,
                    StatusId = @StatusId,
                    ScheduledDate = @ScheduledDate,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(ServiceBooking entity)
        {
            var sql = @"
                UPDATE ServiceBookings SET
                    IsDeleted = 1,
                    DeletedAt = @DeletedAt,
                    DeletedBy = @DeletedBy,
                    DeleteReason = @DeleteReason
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new
            {
                entity.Id,
                DeletedAt = entity.DeletedAt ?? DateTime.UtcNow,
                entity.DeletedBy,
                entity.DeletedReason
            });
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            entity.DeletedBy = "System";
            entity.DeletedReason = "Soft delete by Id";

            await DeleteAsync(entity);
        }
    }
}
