using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface ISecurityParameterRepository
    {
        List<SecurityParameter> Get(SessionContext sessionContext);
        SecurityParameter Get(SessionContext sessionContext, int OrgId);
        SecurityParameter Get(int orgId);
        int Insert(SecurityParameter securityParameter);
        int Update(SecurityParameter updatedSecurityParameter);
        bool Delete(SessionContext sessionContext, int id);
    }
}
