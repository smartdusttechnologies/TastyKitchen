using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface ISecurityParameterService
    {
        List<SecurityParameter> Get(SessionContext sessionContext);
        SecurityParameter Get(SessionContext sessionContext, int OrgId);
        SecurityParameter Get(int OrgId);
        RequestResult<int> Add(SessionContext sessionContext, SecurityParameter securityParameterModel);
        RequestResult<int> Update(SessionContext sessionContext, int id, SecurityParameter updatedSecurityParameter);
        bool Delete(SessionContext sessionContext, int id);
        RequestResult<bool> ValidatePasswordPolicy(SessionContext sessionContext, int orgId, string password);
        RequestResult<bool> ValidatePasswordPolicy(int orgId, string password);
    }
}
