using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Data.Repository;
using Neubel.Wow.Win.Authentication.Services;
using Neubel.Wow.Win.Authentication.WebAPI.Controllers;
using NUnit.Framework;
using Neubel.Wow.Win.Authentication.WebAPI.DTO;
using Neubel.Wow.Win.Authentication.Common;

namespace Neubel.Wow.Win.Authentication.Tests
{
    public class UserTest
    {
        IUserService _userService;
        IUserRepository _userRepository;
        ISecurityParameterRepository _securityParameterRepository;
        ISecurityParameterService _securityParameterService;
        IMapper _mapper;
        WebAPI.DTO.User userToAdd = new WebAPI.DTO.User
        {
            AdminLevel = 0,
            Country = "India",
            Email = "abc@gmail.com",
            EmailValidationStatus = 0,
            FirstName = "abc",
            IsActive = true,
            ISDCode = "91",
            LastName = "cde",
            Locked = false,
            Mobile = "1234567899",
            MobileValidationStatus = 0,
            OrgId = 2,
            TwoFactor = false,
            UserName = "testUser1",
            Password = "Pass@123"
        };

        List<Core.Model.User> users = new List<Core.Model.User>
            {
                new Core.Model.User
                {
                    Id = 1, AdminLevel = 0, Country = "India", Email = "abc@gmail.com", EmailValidationStatus = 0,
                    FirstName = "abc", IsActive = true, ISDCode = "91",
                    LastName = "cde", Locked = false, Mobile = "1234567899", MobileValidationStatus = 0, OrgId = 2,
                    TwoFactor = false, UserName = "testUser1"
                }
            };

        Core.Model.PasswordLogin passwordLogin = new Core.Model.PasswordLogin { PasswordHash = "dfsdfds", PasswordSalt = "fsdfdsfsd", UserId = 1 };

        [SetUp]
        public void Setup()
        {
            // setup test data
            SessionContext sessionContext = new SessionContext();
            //mock mapper.
            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<List<Core.Model.User>, List<WebAPI.DTO.User>>(users)).Returns(MapUsers(users));
            mapper.Setup(m => m.Map<Core.Model.User, WebAPI.DTO.User>(users[0])).Returns(MapUser(users[0]));
            mapper.Setup(m => m.Map<WebAPI.DTO.User, Core.Model.User>(userToAdd)).Returns(users[0]);

            //mock repository\DB calls.
            Mock<IUserRepository> userRepo = new Mock<IUserRepository>();
            userRepo.Setup(mock => mock.Get(new Common.SessionContext())).Returns(users);
            userRepo.Setup(mock => mock.Get(new Common.SessionContext(), 1)).Returns(users[0]);
            userRepo.Setup(mock => mock.Insert(users[0], passwordLogin)).Returns(users[0].Id);
            userRepo.Setup(mock => mock.Delete(new SessionContext(), 1)).Returns(true);
            userRepo.Setup(mock => mock.Update(users[0])).Returns(users[0].Id);

            Mock<ISecurityParameterRepository> securityParameterRepo = new Mock<ISecurityParameterRepository>();
            securityParameterRepo.Setup(m => m.Get(sessionContext, users[0].OrgId)).Returns(GetSecurityParameter());
            
            //mock not required service.
            Mock<ILogger> logger = new Mock<ILogger>();
            logger.Setup(m => m.LogException(new Core.Model.ExceptionLog())).Returns(Task.FromResult(true));
            logger.Setup(m => m.LogTransaction(new Core.Model.TransactionLog())).Returns(Task.FromResult(true));
            logger.Setup(m => m.LogUsage(new Core.Model.UsageLog())).Returns(Task.FromResult(true));
            
            // resolve dependencies.
            _mapper = mapper.Object;
            _userRepository = userRepo.Object;
            _securityParameterRepository = securityParameterRepo.Object;
            _userService = new UserService(_userRepository, _securityParameterRepository, logger.Object, _securityParameterService);
        }

        [Test]
        public void TestGetAllUsers()
        {
            UserController userController = new UserController(_userService, _mapper);
            IActionResult response = userController.Get();
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(1, (data.Value as List<WebAPI.DTO.User>).Count);
        }

        [Test]
        public void TestGetUserById()
        {
            UserController userController = new UserController(_userService, _mapper);
            IActionResult response = userController.Get(1);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual("testUser1", (data.Value as WebAPI.DTO.User).UserName);
        }

        [Test]
        public void TestAddUser()
        {
            UserController userController = new UserController(_userService, _mapper);

            IActionResult response = userController.Post(userToAdd);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.IsTrue((bool)data.Value);
        }

        [Test]
        public void TestUpdateUser()
        {
            UserController userController = new UserController(_userService, _mapper);

            IActionResult response = userController.Put(1, userToAdd);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(0, (int)data.Value);
        }

        [Test]
        public void TestDeleteUser()
        {
            UserController userController = new UserController(_userService, _mapper);

            IActionResult response = userController.Delete(1);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.IsTrue((bool)data.Value);
        }

        #region Private methods
        private List<WebAPI.DTO.User> MapUsers(List<Core.Model.User> users)
        {
            List<WebAPI.DTO.User> list = new List<User>();
            foreach (var user in users)
            {
                list.Add(new User { AdminLevel = user.AdminLevel, Country = user.Country, Email = user.Email, UserName = user.UserName });
            }
            return list;
        }
        private WebAPI.DTO.User MapUser(Core.Model.User user)
        {
            return new User { AdminLevel = user.AdminLevel, Country = user.Country, Email = user.Email, UserName = user.UserName, Password = "Pass@123" };
        }

        private Core.Model.User MapInUser(WebAPI.DTO.User user)
        {
            return new Core.Model.User
            {
                AdminLevel = user.AdminLevel,
                Country = user.Country,
                Email = user.Email,
                UserName = user.UserName,
                OrgId = user.OrgId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailValidationStatus = user.EmailValidationStatus,
                IsActive = user.IsActive,
                ISDCode = user.ISDCode,
                Locked = user.Locked,
                Mobile = user.Mobile,
                MobileValidationStatus = user.MobileValidationStatus,
                TwoFactor = user.TwoFactor

            };
        }

        private Core.Model.SecurityParameter GetSecurityParameter()
        {
            return new Core.Model.SecurityParameter
            {
                Id = 1,
                AllowUserName = true,
                ChangeIntervalDays = 30,
                DisAllowedChars = "",
                DisAllPastPassword = 0,
                MinCaps = 1,
                MinLength = 8,
                MinNumber = 1,
                MinSmallChars = 1,
                MinSpecialChars = 1
            };
        }
        #endregion
    }
}