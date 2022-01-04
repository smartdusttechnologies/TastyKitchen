using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model.Roles;
using Neubel.Wow.Win.Authentication.WebAPI.Common;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityParameterController : NeubelWowBaseApiController
    {
        private readonly ISecurityParameterService _securityParameterService;
        private readonly IMapper _mapper;

        public SecurityParameterController(ISecurityParameterService securityParameterService, IMapper mapper)
        {
            _securityParameterService = securityParameterService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all security parameters/password policies.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorized(AllowedRoles = new[] { UserRoles.ApplicationAdmin, UserRoles.Admin })]
        public IActionResult Get()
        {
            List<Core.Model.SecurityParameter> users = _securityParameterService.Get(SessionContext);
            var securityParameterDto = _mapper.Map<List<Core.Model.SecurityParameter>, List<DTO.SecurityParameter>>(users);
            return Ok(securityParameterDto);
        }
        /// <summary>
        /// Get a security parameters/password policy by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorized(AllowedRoles = new[] { UserRoles.ApplicationAdmin, UserRoles.Admin })]
        public IActionResult Get(int id)
        {
            Core.Model.SecurityParameter user = _securityParameterService.Get(SessionContext, id);
            var securityParameterDto = _mapper.Map<Core.Model.SecurityParameter, DTO.SecurityParameter>(user);
            return Ok(securityParameterDto);
        }

        /// <summary>
        /// Add a security parameters/password policy.
        /// </summary>
        /// <param name="securityParameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Post([FromBody] DTO.SecurityParameter securityParameter)
        {
            var securityParameterModel = _mapper.Map<DTO.SecurityParameter, Core.Model.SecurityParameter>(securityParameter);
            var id = _securityParameterService.Add(SessionContext, securityParameterModel);
            return Ok(id);
        }
        /// <summary>
        /// Update security parameters/password policy.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="securityParameter"></param>
        /// <returns></returns>
        // PUT api/<SecurityParameterController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Put(int id, [FromBody] DTO.SecurityParameter securityParameter)
        {
            var updatedSecurityParameter = _mapper.Map<DTO.SecurityParameter, Core.Model.SecurityParameter>(securityParameter);
            _securityParameterService.Update(SessionContext, id, updatedSecurityParameter);
            return Ok(id);
        }
        /// <summary>
        /// Delete a security parameters/password policy (soft delete).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Delete(int id)
        {
            bool result = _securityParameterService.Delete(SessionContext, id);
            return Ok(result);
        }
    }
}
