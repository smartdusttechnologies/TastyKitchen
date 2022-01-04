using System;
using System.Net.Http;
using System.Text;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using System.Threading.Tasks;
using Neubel.Wow.Win.Authentication.Core.Model;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces;

namespace Neubel.Wow.Win.Authentication.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerRepository _loggerRepository;

        public Logger(IConfiguration configuration, ILoggerRepository loggerRepository)
        {
            _configuration = configuration;
            _loggerRepository = loggerRepository;
        }

        public async Task<bool> LogException(ExceptionLog exceptionLog)
        {
            bool isExceptionLoggingEnabled = Convert.ToBoolean(_configuration["ConnectionStrings:EnableExceptionLog"]);
            if (isExceptionLoggingEnabled)
            {
                var baseUrl = _configuration["ConnectionStrings:WebApiBaseUrl"];
                string endpoint = baseUrl + "/api/Logger/ExceptionLog";
                var content = new StringContent(JsonConvert.SerializeObject(exceptionLog), Encoding.UTF8,
                    "application/json");

                using var client = new HttpClient();
                await client.PostAsync(endpoint, content);
            }
            return true;
        }

        public async Task<bool> LogTransaction(TransactionLog transactionLog)
        {
            bool isTransactionLoggingEnabled = Convert.ToBoolean(_configuration["ConnectionStrings:EnableTransactionLog"]);
            if (isTransactionLoggingEnabled)
            {
                var baseUrl = _configuration["ConnectionStrings:WebApiBaseUrl"];
                string endpoint = baseUrl + "/api/Logger/TransactionLog";
                var content = new StringContent(JsonConvert.SerializeObject(transactionLog), Encoding.UTF8,
                    "application/json");

                using var client = new HttpClient();
                await client.PostAsync(endpoint, content);
            }
            return true;
        }

        public async Task<bool> LogUsage(UsageLog usageLog)
        {
            bool isTransactionLoggingEnabled = Convert.ToBoolean(_configuration["ConnectionStrings:EnableUsageLog"]);
            if (isTransactionLoggingEnabled)
            {
                var baseUrl = _configuration["ConnectionStrings:WebApiBaseUrl"];
                string endpoint = baseUrl + "/api/Logger//api/Logger/api/UsageLog";
                var content = new StringContent(JsonConvert.SerializeObject(usageLog), Encoding.UTF8,
                    "application/json");

                using var client = new HttpClient();
                await client.PostAsync(endpoint, content);
            }
            return true;
        }

        public async Task<int> LockedUserLog(LockUnlockUser lockUnlockUser)
        {
            return await Task.Run(() => _loggerRepository.LockedUserLog(lockUnlockUser));
        }

        public async Task<int> LoginLog(LoginRequest loginRequest)
        {
            return await Task.Run(() => _loggerRepository.LoginLog(loginRequest));
        }

        public async Task<int> LoginTokenLog(LoginToken loginToken)
        {
            return await Task.Run(() => _loggerRepository.LoginTokenLog(loginToken));
        }

        public async Task<int> LoginTokenLogForRefreshToken(RefreshedAccessToken refreshedAccessToken)
        {
            return await Task.Run(() => _loggerRepository.LoginTokenLogForRefreshToken(refreshedAccessToken));
        }

        public async Task<int> PasswordChangeLog(PasswordLogin passwordLogin)
        {
            return await Task.Run(() => _loggerRepository.PasswordChangeLog(passwordLogin));
        }
    }
}
