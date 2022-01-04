using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public UserRoleRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<UserRole> Get(SessionContext sessionContext)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<UserRole>(@"Select ur.UserId, u.UserName, ur.RoleId, r.Name as RoleName  
                                        From [User] u
                                            inner join [UserRole] ur on u.id=ur.userId
                                            inner join [Role] r on ur.roleId=r.id
                                        where (u.OrgId=@OrganizationId OR @OrganizationId = 0) and u.IsDeleted=0 and r.IsDeleted=0 and  and ur.IsDeleted=0", new { sessionContext.OrganizationId }).ToList();
        }
        public List<UserRole> Get(SessionContext sessionContext, int userId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<UserRole>(@"Select ur.UserId, u.UserName, ur.RoleId, r.Name as RoleName  From [User] u
                                            inner join [UserRole] ur on u.id=ur.userId
                                            inner join [Role] r on ur.roleId=r.id
                                           where ur.userId=@userId and (OrgId=@OrganizationId OR @OrganizationId = 0) and u.IsDeleted=0 and r.IsDeleted=0 and  and ur.IsDeleted=0", new{userId, sessionContext.OrganizationId}).ToList();
        }
        public int Insert(UserRole userRole)
        {
            string query = @"Insert into [UserRole](UserId, RoleId) values (@UserId, @RoleId)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, userRole);
        }
        public bool Delete(int id)
        {
            string query = @"update [UserRole] Set 
                                IsDeleted = @IsDeleted
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id });
            return true;
        }
    }
}
