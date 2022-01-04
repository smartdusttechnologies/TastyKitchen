using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface IRoleRepository
    { 
        int Insert(Role role);
        int Update(Role role);
        List<Role> Get();
        Role Get(int id);
        Role GetByName(string roleName);
        List<string> Get(SessionContext sessionContext, string userName);
        List<(int, string)> GetRoleWithOrg(string userName);
        bool Delete(int id);
    }
}
