using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Infrastructure;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public OrganizationRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Public Methods
        public int Insert(Organization organization)
        {
            organization.OrgCode = organization.OrgCode.ToUpper();
            string query = @"Insert into [Organization](OrgCode, OrgName) 
                values (@OrgCode, @OrgName)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, organization);
        }
        public int Update(Organization organization)
        {
            string query = @"update [Organization] Set 
                                OrgCode = @OrgCode, 
                                OrgName = @OrgName
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, organization);
        }
        public List<Organization> Get(SessionContext sessionContext)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<Organization>("Select * From [Organization] where (Id=@OrganizationId OR @OrganizationId = 0) and IsDeleted=0", new { sessionContext.OrganizationId }).ToList();
        }
        public Organization Get(SessionContext sessionContext, int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<Organization>("Select top 1 * From [Organization] where Id=@id and (Id=@OrganizationId OR @OrganizationId = 0) and IsDeleted=0", new { id, sessionContext.OrganizationId }).FirstOrDefault();
        }
        public Organization Get(SessionContext sessionContext, string orgCode)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<Organization>("Select top 1 * From [Organization] where OrgCode=@orgCode and (Id=@OrganizationId OR @OrganizationId = 0) and IsDeleted=0", new { orgCode, sessionContext.OrganizationId }).FirstOrDefault();
        }
        public bool Delete(int id)
        {
            string query = @"update [Organization] Set 
                                IsDeleted = @IsDeleted
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id });
            return true;
        }
        #endregion
    }
}
