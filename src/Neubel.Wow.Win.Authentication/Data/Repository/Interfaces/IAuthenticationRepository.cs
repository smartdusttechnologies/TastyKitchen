using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface IAuthenticationRepository
    {
        List<LoginHistory> GetLoginHistory(int userId);
        int SaveLoginToken(LoginToken loginToken);
        int UpdateAccessToken(RefreshedAccessToken refreshedAccessToken);
        bool UpdatePasswordLogin(PasswordLogin passwordLogin);
        bool LockUnlockUser(LockUnlockUser lockUnlockUser);
        PasswordLogin GetLoginPassword(string userName);
        int SaveOtp(UserValidationOtp userValidationOtp);
        UserValidationOtp GetOtp(int userId);
        bool UpdateMobileConfirmationStatus(int userId, bool mobileValidationStatus);
        bool UpdateEmailConfirmationStatus(int userId, bool emailValidationStatus);
    }
}
