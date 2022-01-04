using System;
using System.Collections.Generic;
using System.Text;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository.Interfaces
{
    public interface ILoggerRepository
    {
        int PasswordChangeLog(PasswordLogin passwordLogin);
        int LoginTokenLog(LoginToken loginToken);
        int LoginTokenLogForRefreshToken(RefreshedAccessToken refreshedAccessToken);
        int LoginLog(LoginRequest loginRequest);
        int LockedUserLog(LockUnlockUser lockUnlockUser);
    }
}
