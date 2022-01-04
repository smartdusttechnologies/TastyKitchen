using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IRoleService
    {
        List<Role> Get();
        Role Get(int id);
        List<string> Get(SessionContext sessionContext, string userName);
        RequestResult<int> Add(Role roleModel);
        RequestResult<int> Update(int id, Role updatedRole);
        bool Delete(int id);
    }
}
