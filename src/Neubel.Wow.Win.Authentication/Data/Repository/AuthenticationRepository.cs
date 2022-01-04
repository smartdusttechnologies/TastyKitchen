using Neubel.Wow.Win.Authentication.Core.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public AuthenticationRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Public Methods.
        public int SaveLoginToken(LoginToken loginToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            int userId = db.Query<int>(@"Select u.Id From [User] u Where u.UserName = @UserName", new { loginToken.UserName}).FirstOrDefault();
            loginToken.UserId = userId;
            int loginTokenUserId = db.Query<int>(@"Select userId From [LoginToken] Where  UserId = @userId", new { userId }).FirstOrDefault();

            string query = loginTokenUserId > 0 ?
                @"update [LoginToken] Set 
                    AccessToken = @AccessToken,
                    RefreshToken = @RefreshToken,
                    AccessTokenExpiry = @AccessTokenExpiry,
                    DeviceCode = @DeviceCode,
                    DeviceName = @DeviceName,
                    RefreshTokenExpiry = @RefreshTokenExpiry
                  Where UserId = @UserId"
                :
                @"Insert into [LoginToken](UserId, AccessToken, RefreshToken, AccessTokenExpiry, DeviceCode, DeviceName, RefreshTokenExpiry) 
                values (@UserId, @AccessToken, @RefreshToken, @AccessTokenExpiry, @DeviceCode, @DeviceName, @RefreshTokenExpiry)";

            return db.Execute(query, loginToken);
        }
        public int UpdateAccessToken(RefreshedAccessToken refreshedAccessToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            int userId = db.Query<int>(@"Select u.Id From [User] u Where u.UserName = @UserName", new { refreshedAccessToken.UserName }).FirstOrDefault();
            refreshedAccessToken.UserId = userId;

            int loginTokenUserId = db.Query<int>(@"Select userId From [LoginToken] Where  UserId = @userId", new { userId }).FirstOrDefault();

            if (loginTokenUserId > 0)
            {
                string query = @"update [LoginToken] Set 
                                UserId = @UserId,
                                AccessToken = @AccessToken,
                                AccessTokenExpiry = @AccessTokenExpiry,
                                DeviceCode = @DeviceCode,
                                DeviceName = @DeviceName
                              Where UserId = @UserId";

                return db.Execute(query, refreshedAccessToken);
            }
            return 0;
        }
        
        public List<LoginHistory> GetLoginHistory(int userId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<LoginHistory>("Select * From [LoginLog] where UserId=@UserId", new { userId }).ToList();
        }
        
        public bool UpdatePasswordLogin(PasswordLogin passwordLogin)
        {
            string query = @"update [PasswordLogin] Set 
                                PasswordHash = @PasswordHash,
                                PasswordSalt = @PasswordSalt
                            Where UserId = @UserId";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, passwordLogin);
            return true;
        }
        public bool LockUnlockUser(LockUnlockUser lockUnlockUser)
        {
            string query = @"update [User] Set
                                Locked = @Locked
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, lockUnlockUser);
            return true;
        }
        public bool UpdateMobileConfirmationStatus(int userId, bool mobileValidationStatus)
        {
            string query = @"update [User] Set
                                MobileValidationStatus = @MobileValidationStatus
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new {Id = userId, MobileValidationStatus = mobileValidationStatus });
            return true;
        }
        public bool UpdateEmailConfirmationStatus(int userId, bool emailValidationStatus)
        {
            string query = @"update [User] Set
                                EmailValidationStatus = @EmailValidationStatus
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { Id = userId, EmailValidationStatus = emailValidationStatus });
            return true;
        }
        public PasswordLogin GetLoginPassword(string userName)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<PasswordLogin>("Select top 1  pl.* From [User] u inner join [PasswordLogin] pl on u.id=pl.userId where u.userName=@userName and (u.IsDeleted=0 AND u.Locked=0 AND u.IsActive=1)", new { userName }).FirstOrDefault();
        }
        public int SaveOtp(UserValidationOtp userValidationOtp)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            int userId = db.Query<int>(@"Select top 1  u.UserId [UserValidationOtp] Where u.UserId = @UserId", new { userValidationOtp.UserId }).FirstOrDefault();

            string query = userId > 0 ?
                @"update [UserValidationOtp] Set 
                    otp = @otp,
                    OtpGeneratedTime = @OtpGeneratedTime,
                    OtpAuthenticatedTime = @OtpAuthenticatedTime,
                    Status = @Status,
                    Type = @Type,
                    OrgId = @OrgId,
                    Where UserId = @UserId"
                :
                @"Insert into [UserValidationOtp](UserId, otp, OtpGeneratedTime, OtpAuthenticatedTime, Status, Type, OrgId) 
                values (@UserId, @otp, @OtpGeneratedTime, @OtpAuthenticatedTime, @Status, @Type, @OrgId)";

            return db.Execute(query, userValidationOtp);
        }
        public UserValidationOtp GetOtp(int userId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<UserValidationOtp>("Select top 1 * From [UserValidationOtp] where UserId=@UserId", new { userId }).FirstOrDefault();
        }
        #endregion
    }
}
