using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface IUserRepository
    {
        int Insert(User user, PasswordLogin passwordLogin);
        int Update(User user);
        List<User> Get(SessionContext sessionContext);
        User Get(SessionContext sessionContext, int id);
        User Get(int id);
        User Get(string userName);
        bool ActivateDeactivateUser(ActivateDeactivateUser activateDeactivateUser);
        bool Delete(SessionContext sessionContext, int id);
        int GetUserOrganization(int userId);
    }
}
