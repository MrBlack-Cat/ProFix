using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;
using System.Security.Claims;

namespace DAL.SqlServer.Infrastructure
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlUserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var sql = "SELECT * FROM Users WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<User>(sql);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task AddAsync(User entity)
        {
            var sql = @"
                INSERT INTO Users (Email, PasswordHash, IsActive, CreatedAt)
                VALUES (@Email, @PasswordHash, @IsActive, @CreatedAt)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(User entity)
        {
            var sql = @"
                UPDATE Users SET 
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    PasswordHash = @PasswordHash,
                    RoleId = @RoleId,
                    IsActive = @IsActive,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy,
                    IsDeleted = @IsDeleted,
                    DeletedAt = @DeletedAt,
                    DeletedBy = @DeletedBy, 
                    DeletedReason = @DeletedReason
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }
        public async Task DeleteAsync(User entity)
        {
            var sql = @"
                UPDATE Users SET 
                    IsDeleted = 1,
                    IsActive = 0,
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


        public async Task DeleteAsync(int id, string deletedReason, int deletedBy)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return;

            user.IsDeleted = true;
            user.IsActive = false;
            user.DeletedAt = DateTime.UtcNow;
            user.DeletedBy = deletedBy;
            user.DeletedReason = deletedReason;

            await DeleteAsync(user);
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            const string sql = @"
            SELECT u.*, r.Id, r.Name
            FROM Users u
            LEFT JOIN UserRole r ON u.RoleId = r.Id
            WHERE u.Email = @Email AND u.IsDeleted = 0
        ";

            var user = await _dbConnection.QueryAsync<User, UserRole, User>(
                sql,
                (user, role) =>
                {
                    user.Role = role;
                    return user;
                },
                new { Email = email },
                splitOn: "Id"
            );

            return user.FirstOrDefault();
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            var sql = "SELECT * FROM Users WHERE UserName = @UserName AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { UserName = username });

            throw new NotImplementedException("UserName field not implemented in Users table");
        }


        public async Task RegisterAsync(User user)
        {
            var sql = @"
        INSERT INTO Users (UserName, Email, PasswordHash, RoleId, CreatedAt)
        VALUES (@UserName, @Email, @PasswordHash, @RoleId, @CreatedAt)";

            await _dbConnection.ExecuteAsync(sql, new
            {
                user.UserName,
                user.Email,
                user.PasswordHash,
                user.RoleId,
                user.CreatedAt
            });
        }

        public async Task DeleteAsync(int id, ClaimsPrincipal user)
        {
            await Task.CompletedTask;
        }



    }
}
