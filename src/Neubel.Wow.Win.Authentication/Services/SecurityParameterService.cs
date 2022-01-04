using System;
using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class SecurityParameterService : ISecurityParameterService
    {
        private readonly ISecurityParameterRepository _securityParameterRepository;
        private readonly ILogger _logger;

        public SecurityParameterService(ISecurityParameterRepository securityParameterRepository, ILogger logger)
        {
            _securityParameterRepository = securityParameterRepository;
            _logger = logger;
        }

        #region Public Methods.
        public List<SecurityParameter> Get(SessionContext sessionContext)
        {
            try
            {
                return _securityParameterRepository.Get(sessionContext);
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
        public SecurityParameter Get(SessionContext sessionContext, int OrgId)
        {
            try
            {
                return _securityParameterRepository.Get(sessionContext, OrgId);
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

        public SecurityParameter Get(int OrgId)
        {
            try
            {
                return _securityParameterRepository.Get(OrgId);
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

        public RequestResult<int> Add(SessionContext sessionContext, SecurityParameter securityParameter)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                if (!Helpers.IsInOrganizationContext(sessionContext, securityParameter.OrgId))
                {
                    validationMessages.Add(new ValidationMessage { Reason = "You are not allowed! Access denied.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }

                SecurityParameter savedSecurityParameter = _securityParameterRepository.Get(sessionContext, securityParameter.OrgId);
                if(savedSecurityParameter != null)
                {
                    validationMessages.Add(new ValidationMessage { Reason = "You cannot add multiple password policy for same organization.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }

                return new RequestResult<int>(_securityParameterRepository.Insert(securityParameter));
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
        public RequestResult<int> Update(SessionContext sessionContext, int id, SecurityParameter securityParameter)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                if (!Helpers.IsInOrganizationContext(sessionContext, securityParameter.OrgId))
                {
                    validationMessages.Add(new ValidationMessage { Reason = "You are not allowed! Access denied.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }

                SecurityParameter savedSecurityParameter = _securityParameterRepository.Get(sessionContext, id);
                if (savedSecurityParameter != null)
                {
                    securityParameter.Id = id;
                    if (!savedSecurityParameter.Equals(securityParameter))
                        return new RequestResult<int> (_securityParameterRepository.Update(securityParameter));
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
        public bool Delete(SessionContext sessionContext, int id)
        {
            try
            {
                return _securityParameterRepository.Delete(sessionContext, id);
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

        public RequestResult<bool> ValidatePasswordPolicy(SessionContext sessionContext, int orgId, string password)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                var passwordPolicy = _securityParameterRepository.Get(orgId);
                var validatePasswordResult = ValidatePassword(password, passwordPolicy);
                return validatePasswordResult;
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
                validationMessages.Add(new ValidationMessage { Reason = "Validation failed!", Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }
        }

        public RequestResult<bool> ValidatePasswordPolicy(int orgId, string password)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                var passwordPolicy = _securityParameterRepository.Get(orgId);
                var validatePasswordResult = ValidatePassword(password, passwordPolicy);
                return validatePasswordResult;
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
                validationMessages.Add(new ValidationMessage { Reason = "Validation failed!", Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }
        }

        #endregion

        #region Private Methods
        private RequestResult<bool> ValidatePassword(string password, SecurityParameter securityParameter)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();

            if (password.Length < securityParameter.MinLength)
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum length of the password should be " + securityParameter.MinLength + "characters long", Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            if (!Helpers.ValidateMinimumSmallChars(password, securityParameter.MinSmallChars))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum number of small characters the password should have is " + securityParameter.MinSmallChars, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            if (!Helpers.ValidateMinimumCapsChars(password, securityParameter.MinCaps))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum number of capital characters the password should have is " + securityParameter.MinCaps, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            if (!Helpers.ValidateMinimumDigits(password, securityParameter.MinNumber))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum number of numeric characters the password should have is " + securityParameter.MinNumber, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            if (!Helpers.ValidateMinimumSpecialChars(password, securityParameter.MinSpecialChars))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum number of special characters the password should have is " + securityParameter.MinSpecialChars, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            if (!Helpers.ValidateDisallowedChars(password, securityParameter.DisAllowedChars))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Characters which are not allowed in password are " + securityParameter.DisAllowedChars, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            return new RequestResult<bool>(true);
        }
        #endregion
    }
}
