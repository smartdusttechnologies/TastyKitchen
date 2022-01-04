using System;
using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ILogger _logger;
        public OrganizationService(IOrganizationRepository organizationRepository, ILogger logger)
        {
            _organizationRepository = organizationRepository;
            _logger = logger;
        }

        #region Public Methods.
        public RequestResult<int> Add(SessionContext sessionContext, Organization organization)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                if (!Helpers.IsInOrganizationContext(sessionContext, organization.Id))
                {
                    validationMessages.Add(new ValidationMessage { Reason = "You are not allowed! Access denied.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }
                var existingOrg = _organizationRepository.Get(sessionContext, organization.OrgCode.ToUpper());
                if (existingOrg != null)
                {
                    validationMessages.Add(new ValidationMessage { Reason = "This OrgCode is not available.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }
                return new RequestResult<int>(_organizationRepository.Insert(organization));
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
                return new RequestResult<int>(0);
            }
        }
        public RequestResult<int> Update(SessionContext sessionContext, int id, Organization organization)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                if (!Helpers.IsInOrganizationContext(sessionContext, organization.Id))
                {
                    validationMessages.Add(new ValidationMessage { Reason = "You are not allowed! Access denied.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }
                var existingOrg = _organizationRepository.Get(sessionContext, organization.OrgCode);
                if (existingOrg != null && existingOrg.Id != id)
                {
                    validationMessages.Add(new ValidationMessage { Reason = "This OrgCode is not available.", Severity = ValidationSeverity.Error });
                    return new RequestResult<int>(0, validationMessages);
                }

                Organization savedOrganization = _organizationRepository.Get(sessionContext, id);
                if (savedOrganization != null)
                {
                    organization.Id = id;
                    if (!savedOrganization.Equals(organization))
                        return new RequestResult<int>(_organizationRepository.Update(organization));
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

            validationMessages.Add(new ValidationMessage { Reason = "Unable to update. Validation failed!", Severity = ValidationSeverity.Error });
            return new RequestResult<int>(0, validationMessages);
            
        }
        public List<Organization> Get(SessionContext sessionContext)
        {
            try
            {
                return _organizationRepository.Get(sessionContext);
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
        public Organization Get(SessionContext sessionContext, int id)
        {
            try
            {
                return _organizationRepository.Get(sessionContext, id);
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
                return _organizationRepository.Delete(id);
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
