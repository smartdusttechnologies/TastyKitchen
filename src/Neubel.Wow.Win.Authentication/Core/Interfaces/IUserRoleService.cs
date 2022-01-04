using System;
using System.Collections.Generic;
using System.Text;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IUserRoleService
    {
        List<UserRole> Get(SessionContext sessionContext);
        List<UserRole> Get(SessionContext sessionContext, int userId);
        int Add(SessionContext sessionContext, UserRole userRole);
        bool Delete(SessionContext sessionContext, int id);
    }
}
