using System;
using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger _logger;
        public RoleService(IRoleRepository roleRepository, ILogger logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        #region Public Methods.
        public RequestResult<int> Add(Role role)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                var existingRole = _roleRepository.GetByName(role.Name);
                if(existingRole !=null)
                {
                    validationMessages.Add(new ValidationMessage { Reason = "This name is not available.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }    
                return new RequestResult<int>(_roleRepository.Insert(role));
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

                validationMessages.Add(new ValidationMessage { Reason = "Unable to add roles. Validation failed!", Severity = ValidationSeverity.Error });
                return new RequestResult<int>(0, validationMessages);
            }
        }
        public RequestResult<int> Update(int id, Role role)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                var existingRole = _roleRepository.GetByName(role.Name);
                if (existingRole != null && existingRole.Id != id)
                {
                    validationMessages.Add(new ValidationMessage { Reason = "This name is not available.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }
                Role savedRole = _roleRepository.Get(id);
                if (savedRole != null)
                {
                    role.Id = id;
                    if (!savedRole.Equals(role))
                        return new RequestResult<int>(_roleRepository.Update(role));
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
            validationMessages.Add(new ValidationMessage { Reason = "Unable to update roles. Validation failed!", Severity = ValidationSeverity.Error });
            return new RequestResult<int>(0);
        }
        public List<Role> Get()
        {
            try
            {
                return _roleRepository.Get();
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
        public Role Get(int id)
        {
            try
            {
                return _roleRepository.Get(id);
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
        public List<string> Get(SessionContext sessionContext, string userName)
        {
            try
            {
                return _roleRepository.Get(sessionContext, userName);
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
        public bool Delete(int id)
        {
            try
            {
                return _roleRepository.Delete(id);
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
    }
}
