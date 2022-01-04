using System;
using System.Collections.Generic;
using System.Text;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository.Interfaces
{
    public interface IUserRoleRepository
    {
        List<UserRole> Get(SessionContext sessionContext);
        List<UserRole> Get(SessionContext sessionContext, int userId);
        int Insert(UserRole userRole);
        bool Delete(int id);
    }
}
