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
    public class SqlGuaranteeDocumentRepository : IGuaranteeDocumentRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlGuaranteeDocumentRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<GuaranteeDocument>> GetAllAsync()
        {
            var sql = "SELECT * FROM GuaranteeDocuments WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<GuaranteeDocument>(sql);
        }

        public async Task<GuaranteeDocument?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM GuaranteeDocuments WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<GuaranteeDocument>(sql, new { Id = id });
        }

        public async Task<IEnumerable<GuaranteeDocument>> GetByClientIdAsync(int clientProfileId)
        {
            var sql = "SELECT * FROM GuaranteeDocuments WHERE ClientProfileId = @ClientProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<GuaranteeDocument>(sql, new { ClientProfileId = clientProfileId });
        }

        public async Task<IEnumerable<GuaranteeDocument>> GetByServiceProviderIdAsync(int serviceProviderProfileId)
        {
            var sql = "SELECT * FROM GuaranteeDocuments WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<GuaranteeDocument>(sql, new { ServiceProviderProfileId = serviceProviderProfileId });
        }

        public async Task AddAsync(GuaranteeDocument entity)
        {
            var sql = @"
                INSERT INTO GuaranteeDocuments (ServiceProviderProfileId, ClientProfileId, Title, Description, FileUrl, CreatedAt, CreatedBy)
                VALUES (@ServiceProviderProfileId, @ClientProfileId, @Title, @Description, @FileUrl, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(GuaranteeDocument entity)
        {
            var sql = @"
                UPDATE GuaranteeDocuments SET
                    Title = @Title,
                    Description = @Description,
                    FileUrl = @FileUrl,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(GuaranteeDocument entity)
        {
            var sql = @"
                UPDATE GuaranteeDocuments SET
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
