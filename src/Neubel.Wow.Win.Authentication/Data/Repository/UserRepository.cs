using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class UserRepository : IUserRepository
    { 
        private readonly IConnectionFactory _connectionFactory;
        public UserRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Public Methods
        public int Insert(User user, PasswordLogin passwordLogin)
        {
            var p = new DynamicParameters();
            p.Add("Id", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@UserName", user.UserName);
            p.Add("@FirstName", user.FirstName);
            p.Add("@LastName", user.LastName);
            p.Add("@Email", user.Email);
            p.Add("@Mobile", user.Mobile);
            p.Add("@Country", user.Country);
            p.Add("@ISDCode", user.ISDCode);
            p.Add("@TwoFactor", user.TwoFactor);
            p.Add("@Locked", user.Locked);
            p.Add("@IsActive", user.IsActive);
            p.Add("@EmailValidationStatus", user.EmailValidationStatus);
            p.Add("@MobileValidationStatus", user.MobileValidationStatus);
            p.Add("@OrgId", user.OrgId);
            p.Add("@AdminLevel", user.AdminLevel);

            string userInsertQuery = @"Insert into [User](UserName, FirstName, LastName, Email, Mobile, Country, ISDCode, TwoFactor, Locked, IsActive, EmailValidationStatus, MobileValidationStatus, OrgId, AdminLevel) 
                values (@UserName, @FirstName, @LastName, @Email, @Mobile, @Country, @ISDCode, @TwoFactor, @Locked, @IsActive, @EmailValidationStatus, @MobileValidationStatus, @OrgId, @AdminLevel);
                SELECT @Id = @@IDENTITY";

            string passwordLoginInsertQuery = @"Insert into [PasswordLogin](PasswordHash, PasswordSalt, UserId, ChangeDate) 
                values (@PasswordHash, @PasswordSalt, @UserId, @ChangeDate)";

            string userRoleInsertQuery = @"Insert into [UserRole](UserId, RoleId) values (@UserId, @RoleId)";

            using IDbConnection db = _connectionFactory.GetConnection;
            using var transaction = db.BeginTransaction();
            db.Execute(userInsertQuery, p, transaction);

            int insertedUserId = p.Get<int>("@Id");

            passwordLogin.UserId = insertedUserId;
            passwordLogin.ChangeDate = DateTime.Now;
            db.Execute(passwordLoginInsertQuery, passwordLogin, transaction);

            // assign the general user role by default.
            db.Execute(userRoleInsertQuery, new { UserId = insertedUserId, RoleId = 3 }, transaction);
            transaction.Commit();

            return insertedUserId;
        }
        public int Update(User user)
        {
            string query = @"update [User] Set 
                                UserName = @UserName, 
                                FirstName = @FirstName,
                                LastName = @LastName,
                                Email = @Email,
                                Mobile = @Mobile,
                                Country = @Country,
                                ISDCode = @ISDCode,
                                TwoFactor = @TwoFactor,
                                Locked = @Locked,
                                IsActive = @IsActive,
                                EmailValidationStatus = @EmailValidationStatus,
                                MobileValidationStatus = @MobileValidationStatus,
                                OrgId = @OrgId,
                                AdminLevel = @AdminLevel 
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, user);
        }
        public List<User> Get(SessionContext sessionContext)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<User>("Select * From [User] where (OrgId=@OrganizationId OR @OrganizationId = 0) and IsDeleted=0", new { sessionContext.OrganizationId }).ToList();
        }
        public User Get(SessionContext sessionContext, int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<User>("Select top 1 * From [User] where Id=@id and (OrgId=@OrganizationId OR @OrganizationId = 0) and IsDeleted=0", new {id, sessionContext.OrganizationId }).FirstOrDefault();
        }

        public User Get(int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<User>("Select top 1 * From [User] where Id=@id and IsDeleted=0", new { id }).FirstOrDefault();
        }
        public bool ActivateDeactivateUser(ActivateDeactivateUser activateDeactivateUser)
        {
            string query = @"update [User] Set 
                                IsActive = @IsActive
                            Where UserName = @UserName and ( Id=@id and OrgId=@OrganizationId OR @OrganizationId = 0)";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, activateDeactivateUser);
            return true;
        }
        public bool Delete(SessionContext sessionContext, int id)
        {
            string query = @"update [User] Set 
                                IsDeleted = @IsDeleted
                            Where Id = @Id and  Id=@id and (OrgId=@OrganizationId OR @OrganizationId = 0)";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id, sessionContext.OrganizationId });
            return true;
        }
        public int GetUserOrganization(int userId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<int>("Select top 1 OrgId From [USER] where Id=@id and IsDeleted=0", new { id = userId }).FirstOrDefault();
        }
        public User Get(string userName)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<User>("Select top 1 * From [USER] where UserName=@userName and IsDeleted=0", new { userName }).FirstOrDefault();
        }
        #endregion
    }
}
