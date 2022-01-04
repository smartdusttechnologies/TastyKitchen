using System;
using System.Data;
using Dapper;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class LoggerRepository : ILoggerRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public LoggerRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public int PasswordChangeLog(PasswordLogin passwordLogin)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = @"Insert into [PasswordLog](UserId, PasswordHash, PasswordSalt, ChangeDate) 
                values (@UserId, @PasswordHash, @PasswordSalt, @ChangeDate)";

            return db.Execute(query, new { passwordLogin.UserId, passwordLogin.PasswordHash, passwordLogin.PasswordSalt, ChangeDate = DateTime.Now });
        }
        public int LoginTokenLog(LoginToken loginToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = @"Insert into [LoginTokenLog](UserId, AccessToken, RefreshToken, AccessTokenExpiry, DeviceCode, DeviceName, RefreshTokenExpiry) 
                values (@UserId, @AccessToken, @RefreshToken, @AccessTokenExpiry, @DeviceCode, @DeviceName, @RefreshTokenExpiry)";

            return db.Execute(query, loginToken);
        }
        public int LoginTokenLogForRefreshToken(RefreshedAccessToken refreshedAccessToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = @"Insert into [LoginTokenLog](UserId, AccessToken, AccessTokenExpiry, DeviceCode, DeviceName) 
                values (@UserId, @AccessToken, @AccessTokenExpiry, @DeviceCode, @DeviceName)";

            return db.Execute(query, refreshedAccessToken);
        }
        public int LoginLog(LoginRequest loginRequest)
        {
            string query = @"Insert into [LoginLog](UserId, LoginDate, Status, UserName, PasswordHash, IPAddress, Browser, DeviceCode, DeviceName) 
                values (@Id, @LoginDate, @Status, @UserName, @PasswordHash, @IPAddress, @Browser, @DeviceCode, @DeviceName)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, loginRequest);
        }
        public int LockedUserLog(LockUnlockUser lockUnlockUser)
        {
            string query = @"insert into [LockedLog] (LockedDate, UserId) values (@LockedDate, @UserId)";
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, new { LockedDate = DateTime.Now, UserId = lockUnlockUser.Id });
        }
    }
}
