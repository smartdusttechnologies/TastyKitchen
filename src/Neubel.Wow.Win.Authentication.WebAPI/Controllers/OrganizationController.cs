using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication.Core.Model.Roles;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : NeubelWowBaseApiController
    {
        private readonly Core.Interfaces.IOrganizationService _organizationService;
        private readonly IMapper _mapper;

        public OrganizationController(Core.Interfaces.IOrganizationService organizationService, IMapper mapper)
        {
            _organizationService = organizationService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all organization.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.Sysadmin + "," + UserRoles.Admin)]
        [HttpGet]
        public IActionResult Get()
        {
            List<Core.Model.Organization> organizations = _organizationService.Get(SessionContext);
            var organizationsDto = _mapper.Map<List<Core.Model.Organization>, List<DTO.Organization>>(organizations);
            return Ok(organizationsDto);
        }
        /// <summary>
        /// Get organization by organization Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.Sysadmin + "," + UserRoles.Admin)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Core.Model.Organization organization = _organizationService.Get(SessionContext, id);
            var organizationDto = _mapper.Map<Core.Model.Organization, DTO.Organization>(organization);
            return Ok(organizationDto);
        }
        /// <summary>
        /// Add new organization
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.Sysadmin)]
        [HttpPost]
        public IActionResult Post(DTO.Organization organization)
        {
            var organizationModel = _mapper.Map<DTO.Organization, Core.Model.Organization>(organization);
            var result = _organizationService.Add(SessionContext, organizationModel);

            if (result.IsSuccessful)
                return Ok(result.RequestedObject);

            return Ok(result.ValidationMessages);
        }
        /// <summary>
        /// Update existing organization by organization id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        // PUT api/<OrganizationController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Sysadmin)]
        public IActionResult Put(int id, [FromBody] DTO.Organization organization)
        {
            var updatedOrganization = _mapper.Map<DTO.Organization, Core.Model.Organization>(organization);
            var result = _organizationService.Update(SessionContext, id, updatedOrganization);

            if (result.IsSuccessful)
                return Ok(result.RequestedObject);

            return Ok(result.ValidationMessages);
        }
        /// <summary>
        /// Delete an organization (soft delete).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Sysadmin)]
        public IActionResult Delete(int id)
        {
            bool result = _organizationService.Delete(id);
            return Ok(result);
        }
    }
}
