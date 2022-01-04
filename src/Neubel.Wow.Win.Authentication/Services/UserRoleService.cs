using System;
using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public UserRoleService(IUserRoleRepository userRoleRepository, ILogger logger, IUserRepository userRepository)
        {
            _userRoleRepository = userRoleRepository;
            _logger = logger;
            _userRepository = userRepository;
        }

        public int Add(SessionContext sessionContext, UserRole userRole)
        {
            try
            {
                int userOrganizationId = _userRepository.GetUserOrganization(userRole.UserId);
                if (sessionContext.OrganizationId != userOrganizationId)
                    return 0;

                return _userRoleRepository.Insert(userRole);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return 0;
            }
        }

        public bool Delete(SessionContext sessionContext, int id)
        {
            try
            {
                return _userRoleRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return false;
            }
        }

        public List<UserRole> Get(SessionContext sessionContext)
        {
            try
            {
                return _userRoleRepository.Get(sessionContext);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return null;
            }
        }

        public List<UserRole> Get(SessionContext sessionContext, int userId)
        {
            try
            {
                return _userRoleRepository.Get(sessionContext, userId);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return null;
            }
        }
    }
}
