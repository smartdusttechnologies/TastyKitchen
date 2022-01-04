using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Data.Repository;
using Neubel.Wow.Win.Authentication.Services;
using Neubel.Wow.Win.Authentication.WebAPI.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Neubel.Wow.Win.Authentication.Tests
{
    public class OrganizationTest
    {
        IOrganizationService _organizationService;
        IOrganizationRepository _organizationRepository;
        IMapper _mapper;


        WebAPI.DTO.Organization organizationToAdd = new WebAPI.DTO.Organization
        {
             OrgCode = "TestOrg1",
             OrgName = "TestOrg1"
        };

        List<Core.Model.Organization> organizations = new List<Core.Model.Organization>
        {
            new Core.Model.Organization
            {
                Id = 1,
                OrgCode = "TestOrg1",
                OrgName = "TestOrg1"
            }
        };

        [SetUp]
        public void Setup()
        {
            // setup test data
            SessionContext sessionContext = new SessionContext();
            //mock mapper.
            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<List<Core.Model.Organization>, List<WebAPI.DTO.Organization>>(organizations)).Returns(MapOrganizations(organizations));
            mapper.Setup(m => m.Map<Core.Model.Organization, WebAPI.DTO.Organization>(organizations[0])).Returns(MapOrganization(organizations[0]));
            mapper.Setup(m => m.Map<WebAPI.DTO.Organization, Core.Model.Organization>(organizationToAdd)).Returns(organizations[0]);

            //mock repository\DB calls.
            Mock<IOrganizationRepository> organizationRepo = new Mock<IOrganizationRepository>();
            organizationRepo.Setup(mock => mock.Get(sessionContext)).Returns(organizations);
            organizationRepo.Setup(mock => mock.Get(sessionContext, 1)).Returns(organizations[0]);
            organizationRepo.Setup(mock => mock.Insert(organizations[0])).Returns(organizations[0].Id);
            organizationRepo.Setup(mock => mock.Delete(1)).Returns(true);

            //mock not required service.
            Mock<ILogger> logger = new Mock<ILogger>();
            logger.Setup(m => m.LogException(new Core.Model.ExceptionLog())).Returns(Task.FromResult(true));
            logger.Setup(m => m.LogTransaction(new Core.Model.TransactionLog())).Returns(Task.FromResult(true));
            logger.Setup(m => m.LogUsage(new Core.Model.UsageLog())).Returns(Task.FromResult(true));

            // resolve dependencies.
            _mapper = mapper.Object;
            _organizationRepository = organizationRepo.Object;
            _organizationService = new OrganizationService(_organizationRepository, logger.Object);
        }

        [Test]
        public void TestGetAllOrganizations()
        {
            OrganizationController organizationController = new OrganizationController(_organizationService, _mapper);
            IActionResult response = organizationController.Get();
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(1, (data.Value as List<WebAPI.DTO.Organization>).Count);
        }

        [Test]
        public void TestGetOrganizationById()
        {
            OrganizationController organizationController = new OrganizationController(_organizationService, _mapper);
            IActionResult response = organizationController.Get(1);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual("TestOrg1", (data.Value as WebAPI.DTO.Organization).OrgName);
        }

        [Test]
        public void TestAddOrganization()
        {
            OrganizationController organizationController = new OrganizationController(_organizationService, _mapper);

            IActionResult response = organizationController.Post(organizationToAdd);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
        }
        [Test]
        public void TestDeleteOrganization()
        {
            OrganizationController organizationController = new OrganizationController(_organizationService, _mapper);

            IActionResult response = organizationController.Delete(1);
            OkObjectResult data = response as OkObjectResult;
            Assert.AreEqual(data.StatusCode, (int)HttpStatusCode.OK);
            Assert.IsTrue((bool)data.Value);
        }

        #region Private methods
        private List<WebAPI.DTO.Organization> MapOrganizations(List<Core.Model.Organization> Organizations)
        {
            List<WebAPI.DTO.Organization> list = new List<WebAPI.DTO.Organization>();
            foreach (var organization in Organizations)
            {
                list.Add(new WebAPI.DTO.Organization {  OrgName = organization.OrgName,  OrgCode = organization.OrgCode });
            }
            return list;
        }
        private WebAPI.DTO.Organization MapOrganization(Core.Model.Organization organization)
        {
            return new WebAPI.DTO.Organization { OrgName = organization.OrgName, OrgCode = organization.OrgCode };
        }
        #endregion
    }
}
