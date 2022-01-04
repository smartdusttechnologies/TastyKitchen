using System.Threading.Tasks;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface ILogger
    {
        Task<bool> LogException(ExceptionLog exceptionLog);
        Task<bool> LogUsage(UsageLog usageLog);
        Task<bool> LogTransaction(TransactionLog transactionLog);
        Task<int> PasswordChangeLog(PasswordLogin passwordLogin);
        Task<int> LoginTokenLog(LoginToken loginToken);
        Task<int> LoginTokenLogForRefreshToken(RefreshedAccessToken refreshedAccessToken);
        Task<int> LoginLog(LoginRequest loginRequest);
        Task<int> LockedUserLog(LockUnlockUser lockUnlockUser);
    }
}
