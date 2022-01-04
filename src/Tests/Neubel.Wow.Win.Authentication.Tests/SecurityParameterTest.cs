using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Data.Repository;
using Neubel.Wow.Win.Authentication.Services;
using Neubel.Wow.Win.Authentication.WebAPI.Controllers;
using NUnit.Framework;

namespace Neubel.Wow.Win.Authentication.Tests
{
    public class SecurityParameterTest
    {
        ISecurityParameterService _securityParameterService;
        ISecurityParameterRepository _securityParameterRepository;
        IMapper _mapper;


        WebAPI.DTO.SecurityParameter securityParameterToAdd = new WebAPI.DTO.SecurityParameter
        {
            AllowUserName = true,
            ChangeIntervalDays = 30,
            DisAllowedChars = "",
            DisAllPastPassword = 0,
            MinCaps = 1,
            MinLength = 8,
            MinNumber = 1,
            MinSmallChars = 1,
            MinSpecialChars = 1,
            OrgId = 2
        };


        List<Core.Model.SecurityParameter> securityParameters = new List<Core.Model.SecurityParameter>
        {
            new Core.Model.SecurityParameter
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
                MinSpecialChars = 1,
                OrgId = 2
            }
        };

        [SetUp]
        public void Setup()
        {
            // setup test data
            SessionContext sessionContext = new SessionContext();
            //mock mapper.
            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<List<Core.Model.SecurityParameter>, List<WebAPI.DTO.SecurityParameter>>(securityParameters)).Returns(MapSecurityParameters(securityParameters));
            mapper.Setup(m => m.Map<Core.Model.SecurityParameter, WebAPI.DTO.SecurityParameter>(securityParameters[0])).Returns(MapSecurityParameter(securityParameters[0]));
            mapper.Setup(m => m.Map<WebAPI.DTO.SecurityParameter, Core.Model.SecurityParameter>(securityParameterToAdd)).Returns(securityParameters[0]);

            //mock repository\DB calls.
            Mock<ISecurityParameterRepository> securityParameterRepo = new Mock<ISecurityParameterRepository>();
            securityParameterRepo.Setup(mock => mock.Get(sessionContext)).Returns(securityParameters);
            securityParameterRepo.Setup(mock => mock.Get(sessionContext, 1)).Returns(securityParameters[0]);
            securityParameterRepo.Setup(mock => mock.Insert(securityParameters[0])).Returns(securityParameters[0].Id);
            securityParameterRepo.Setup(mock => mock.Delete(new SessionContext(), 1)).Returns(true);
            
            //mock not required service.
            Mock<ILogger> logger = new Mock<ILogger>();
            logger.Setup(m => m.LogException(new Core.Model.ExceptionLog())).Returns(Task.FromResult(true));
            logger.Setup(m => m.LogTransaction(new Core.Model.TransactionLog())).Returns(Task.FromResult(true));
            logger.Setup(m => m.LogUsage(new Core.Model.UsageLog())).Returns(Task.FromResult(true));
            
            // resolve dependencies.
            _mapper = mapper.Object;
            _securityParameterRepository = securityParameterRepo.Object;
            _securityParameterService = new SecurityParameterService(_securityParameterRepository, logger.Object);
        }

        [Test]
        public void TestGetAllSecurityParameters()
        {
            SecurityParameterController securityParameterController = new SecurityParameterController(_securityParameterService, _mapper);
            IActionResult response = securityParameterController.Get();
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(1, (data.Value as List<WebAPI.DTO.SecurityParameter>).Count);
        }

        [Test]
        public void TestGetSecurityParameterById()
        {
            SecurityParameterController securityParameterController = new SecurityParameterController(_securityParameterService, _mapper);
            IActionResult response = securityParameterController.Get(1);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(8, (data.Value as WebAPI.DTO.SecurityParameter).MinLength);
            Assert.AreEqual(1, (data.Value as WebAPI.DTO.SecurityParameter).MinCaps);
            Assert.AreEqual(1, (data.Value as WebAPI.DTO.SecurityParameter).MinNumber);
            Assert.AreEqual(1, (data.Value as WebAPI.DTO.SecurityParameter).MinSmallChars);
            Assert.AreEqual(1, (data.Value as WebAPI.DTO.SecurityParameter).MinSpecialChars);
        }

        [Test]
        public void TestAddSecurityParameter()
        {
            SecurityParameterController securityParameterController = new SecurityParameterController(_securityParameterService, _mapper);

            IActionResult response = securityParameterController.Post(securityParameterToAdd);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
        }

        [Test]
        public void TestDeleteSecurityParameter()
        {
            SecurityParameterController securityParameterController = new SecurityParameterController(_securityParameterService, _mapper);

            IActionResult response = securityParameterController.Delete(1);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.IsTrue((bool)data.Value);
        }
        #region Private methods
        private List<WebAPI.DTO.SecurityParameter> MapSecurityParameters(List<Core.Model.SecurityParameter> SecurityParameters)
        {
            List<WebAPI.DTO.SecurityParameter> list = new List<WebAPI.DTO.SecurityParameter>();
            foreach (var securityParameter in SecurityParameters)
            {
                list.Add(new WebAPI.DTO.SecurityParameter 
                {
                    AllowUserName = securityParameter.AllowUserName,
                    ChangeIntervalDays = securityParameter.ChangeIntervalDays,
                    DisAllowedChars = securityParameter.DisAllowedChars,
                    DisAllPastPassword = securityParameter.MinCaps,
                    MinCaps = securityParameter.MinCaps,
                    MinLength = securityParameter.MinLength,
                    MinNumber = securityParameter.MinNumber,
                    MinSmallChars = securityParameter.MinSmallChars,
                    MinSpecialChars = securityParameter.MinSpecialChars,
                    OrgId = securityParameter.OrgId
                });
            }
            return list;
        }
        private WebAPI.DTO.SecurityParameter MapSecurityParameter(Core.Model.SecurityParameter securityParameter)
        {
            return new WebAPI.DTO.SecurityParameter 
            {
                AllowUserName = securityParameter.AllowUserName,
                ChangeIntervalDays = securityParameter.ChangeIntervalDays,
                DisAllowedChars = securityParameter.DisAllowedChars,
                DisAllPastPassword = securityParameter.MinCaps,
                MinCaps = securityParameter.MinCaps,
                MinLength = securityParameter.MinLength,
                MinNumber = securityParameter.MinNumber,
                MinSmallChars = securityParameter.MinSmallChars,
                MinSpecialChars = securityParameter.MinSpecialChars,
                OrgId = securityParameter.OrgId
            };
        }
        #endregion
    }
}
