using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication.Core.Model.Roles;
using Neubel.Wow.Win.Authentication.WebAPI.Common;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : NeubelWowBaseApiController
    {
        private readonly Core.Interfaces.IUserRoleService _userRoleService;
        private readonly IMapper _mapper;

        public UserRoleController(Core.Interfaces.IUserRoleService userRoleService, IMapper mapper)
        {
            _userRoleService = userRoleService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all user roles.
        /// </summary>
        /// <returns></returns>
        [Authorized(AllowedRoles = new[] { UserRoles.Sysadmin, UserRoles.ApplicationAdmin, UserRoles.Admin })]
        [HttpGet]
        public IActionResult Get()
        {
            List<Core.Model.UserRole> userRoles = _userRoleService.Get(SessionContext);
            var userRolesDto = _mapper.Map<List<Core.Model.UserRole>, List<DTO.UserRole>>(userRoles);
            return Ok(userRolesDto);
        }
        /// <summary>
        /// Get user roles by user Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorized(AllowedRoles = new[] { UserRoles.Sysadmin, UserRoles.ApplicationAdmin, UserRoles.Admin })]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            List<Core.Model.UserRole> userRoles = _userRoleService.Get(SessionContext, id);
            var userRolesDto = _mapper.Map< List<Core.Model.UserRole>, List<DTO.UserRole>>(userRoles);
            return Ok(userRolesDto);
        }

        [Authorized(AllowedRoles = new[] { UserRoles.Sysadmin, UserRoles.Admin })]
        [HttpPost]
        public IActionResult Post(DTO.UserRole userRole)
        {
            var userRoleModel = _mapper.Map<DTO.UserRole, Core.Model.UserRole>(userRole);
            int id = _userRoleService.Add(SessionContext, userRoleModel);
            return Ok(id);
        }

        [HttpDelete("{id}")]
        [Authorized(AllowedRoles = new[] { UserRoles.Sysadmin, UserRoles.Admin })]
        public IActionResult Delete(int id)
        {
            bool result = _userRoleService.Delete(SessionContext, id);
            return Ok(result);
        }
    }
}
