using System;
using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityParameterRepository _securityParameterRepository;
        private readonly ILogger _logger;
        private readonly ISecurityParameterService _securityParameterService;

        public UserService(
            IUserRepository userRepository,
            ISecurityParameterRepository securityParameterRepository,
            ILogger logger,
            ISecurityParameterService securityParameterService)
        {
            _userRepository = userRepository;
            _securityParameterRepository = securityParameterRepository;
            _logger = logger;
            _securityParameterService = securityParameterService;
        }

        #region Public Methods.
        public RequestResult<bool> Add(SessionContext sessionContext, User user, string password)
        {
            try
            {
                var validationResult = ValidateNewUserRegistration(sessionContext, user, password);
                if (validationResult.IsSuccessful)
                {
                    PasswordLogin passwordLogin = Hasher.HashPassword(password);
                    _userRepository.Insert(user, passwordLogin);
                    return new RequestResult<bool>(true);
                }

                return new RequestResult<bool>(false, validationResult.ValidationMessages);
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
                return new RequestResult<bool>(false);
            }
        }
        public RequestResult<int> Update(SessionContext sessionContext, int id, User user)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                if (!Helpers.IsInOrganizationContext(sessionContext, user.OrgId))
                {
                    validationMessages.Add(new ValidationMessage { Reason = "You are not allowed! Access denied.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }

                User existingUser = _userRepository.Get(user.UserName);
                if (existingUser != null && existingUser.Id != id)
                {
                    var error = new ValidationMessage { Reason = "The UserName not available", Severity = ValidationSeverity.Error };
                    validationMessages.Add(error);
                    return new RequestResult<int>(0, validationMessages);
                }
                User savedUser = _userRepository.Get(sessionContext, id);
                if (savedUser != null)
                {
                    user.Id = id;
                    if (!savedUser.Equals(user))
                        return new RequestResult<int>(_userRepository.Update(user));
                }
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
            }

            validationMessages.Add(new ValidationMessage { Reason = "Validation failed!", Severity = ValidationSeverity.Error });
            return new RequestResult<int>(0, validationMessages);
        }
        public List<User> Get(SessionContext sessionContext)
        {
            try
            {
                return _userRepository.Get(sessionContext);
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
        public User Get(SessionContext sessionContext, int id)
        {
            try
            {
                return _userRepository.Get(sessionContext, id);
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
        public bool ActivateDeactivateUser(SessionContext sessionContext, ActivateDeactivateUser activateDeactivateUser)
        {
            try
            {
                return _userRepository.ActivateDeactivateUser(activateDeactivateUser);
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
        public bool Delete(SessionContext sessionContext, int id)
        {
            try
            {
                return _userRepository.Delete(sessionContext, id);
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

        #endregion

        #region Private Methods.
        private RequestResult<bool> ValidateNewUserRegistration(SessionContext sessionContext, User user, string password)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            User existingUser = _userRepository.Get(user.UserName);
            if (existingUser != null)
            {
                var error = new ValidationMessage { Reason = "The UserName not available", Severity = ValidationSeverity.Error };
                validationMessages.Add(error);
                return new RequestResult<bool>(false, validationMessages);
            }

            //if (!Helpers.IsInOrganizationContext(sessionContext, user.OrgId))
            //{
            //    var error = new ValidationMessage { Reason = "You can only register users in your organization", Severity = ValidationSeverity.Error };
            //    validationMessages.Add(error);
            //    return new RequestResult<bool>(false, validationMessages);
            //}
            var validatePasswordResult = _securityParameterService.ValidatePasswordPolicy(sessionContext, user.OrgId, password);
            return validatePasswordResult;
        }

        
        #endregion

    }
}
