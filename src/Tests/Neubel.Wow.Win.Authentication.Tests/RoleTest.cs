using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Data.Repository;
using Neubel.Wow.Win.Authentication.Services;
using Neubel.Wow.Win.Authentication.WebAPI.Controllers;
using NUnit.Framework;

namespace Neubel.Wow.Win.Authentication.Tests
{
    public class RoleTest
    {
        IRoleService _roleService;
        IRoleRepository _roleRepository;
        IMapper _mapper;


        WebAPI.DTO.Role roleToAdd = new WebAPI.DTO.Role
        {
            Name = "Admin",
            Level = 0
        };

        List<Core.Model.Role> roles = new List<Core.Model.Role>
        {
            new Core.Model.Role
            {
                    Id = 1, Name = "Admin", Level = 0
            }
        };

        [SetUp]
        public void Setup()
        {
            // setup test data
            //mock mapper.
            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<List<Core.Model.Role>, List<WebAPI.DTO.Role>>(roles)).Returns(MapRoles(roles));
            mapper.Setup(m => m.Map<Core.Model.Role, WebAPI.DTO.Role>(roles[0])).Returns(MapRole(roles[0]));
            mapper.Setup(m => m.Map<WebAPI.DTO.Role, Core.Model.Role>(roleToAdd)).Returns(roles[0]);

            //mock repository\DB calls.
            Mock<IRoleRepository> roleRepo = new Mock<IRoleRepository>();
            roleRepo.Setup(mock => mock.Get()).Returns(roles);
            roleRepo.Setup(mock => mock.Get(1)).Returns(roles[0]);
            roleRepo.Setup(mock => mock.Insert(roles[0])).Returns(roles[0].Id);
            roleRepo.Setup(mock => mock.Delete(1)).Returns(true);
            roleRepo.Setup(mock => mock.Update(roles[0])).Returns(roles[0].Id);

            //mock not required service.
            Mock<ILogger> logger = new Mock<ILogger>();
            logger.Setup(m => m.LogException(new Core.Model.ExceptionLog())).Returns(Task.FromResult(true));
            logger.Setup(m => m.LogTransaction(new Core.Model.TransactionLog())).Returns(Task.FromResult(true));
            logger.Setup(m => m.LogUsage(new Core.Model.UsageLog())).Returns(Task.FromResult(true));

            // resolve dependencies.
            _mapper = mapper.Object;
            _roleRepository = roleRepo.Object;
            _roleService = new RoleService(_roleRepository, logger.Object);
        }

        [Test]
        public void TestGetAllRoles()
        {
            RoleController roleController = new RoleController(_roleService, _mapper);
            IActionResult response = roleController.Get();
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(1, (data.Value as List<WebAPI.DTO.Role>).Count);
        }

        [Test]
        public void TestGetRoleById()
        {
            RoleController roleController = new RoleController(_roleService, _mapper);
            IActionResult response = roleController.Get(1);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual("Admin", (data.Value as WebAPI.DTO.Role).Name);
        }

        [Test]
        public void TestAddRole()
        {
            RoleController roleController = new RoleController(_roleService, _mapper);

            IActionResult response = roleController.Post(roleToAdd);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(1, (int)data.Value);
        }

        [Test]
        public void TestUpdateRole()
        {
            RoleController roleController = new RoleController(_roleService, _mapper);

            IActionResult response = roleController.Put(1, roleToAdd);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(1, (int)data.Value);
        }

        [Test]
        public void TestDeleteRole()
        {
            RoleController roleController = new RoleController(_roleService, _mapper);

            IActionResult response = roleController.Delete(1);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.IsTrue((bool)data.Value);
        }

        #region Private methods
        private List<WebAPI.DTO.Role> MapRoles(List<Core.Model.Role> Roles)
        {
            List<WebAPI.DTO.Role> list = new List<WebAPI.DTO.Role>();
            foreach (var role in Roles)
            {
                list.Add(new WebAPI.DTO.Role {  Name = role.Name, Level = role.Level });
            }
            return list;
        }
        private WebAPI.DTO.Role MapRole(Core.Model.Role role)
        {
            return new WebAPI.DTO.Role { Name = role.Name, Level = role.Level };
        }
        #endregion
    }
}
