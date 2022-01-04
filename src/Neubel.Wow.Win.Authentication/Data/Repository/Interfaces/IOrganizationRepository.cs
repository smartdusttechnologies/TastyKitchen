using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface IOrganizationRepository
    {
        int Insert(Organization user);
        int Update(Organization user);
        List<Organization> Get(SessionContext sessionContext);
        Organization Get(SessionContext sessionContext, int id);
        Organization Get(SessionContext sessionContext, string orgCode);
        bool Delete(int id);
    }
}
