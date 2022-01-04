using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Insert user.
        /// </summary>
        /// <param name="authorization"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        RequestResult<bool> Add(SessionContext SessionContext, User user, string password);

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        RequestResult<int> Update(SessionContext sessionContext, int id, User user);
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        List<User> Get(SessionContext sessionContext);
        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User Get(SessionContext sessionContext, int id);
        bool ActivateDeactivateUser(SessionContext sessionContext, ActivateDeactivateUser activateDeactivateUser);
        bool Delete(SessionContext sessionContext, int id);
    }
}
