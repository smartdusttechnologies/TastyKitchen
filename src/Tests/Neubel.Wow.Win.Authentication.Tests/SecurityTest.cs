//using System.Collections.Generic;
//using System.Net;
//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Neubel.Wow.Win.Authentication.Core.Interfaces;
//using Neubel.Wow.Win.Authentication.Data.Repository;
//using Neubel.Wow.Win.Authentication.Services;
//using Neubel.Wow.Win.Authentication.WebAPI.Controllers;
//using NUnit.Framework;

//namespace Neubel.Wow.Win.Authentication.Tests
//{
//    public class SecurityTest
//    {
//        //IAuthenticationService _authenticationService;
//        //IAuthenticationRepository _authenticationRepository;
//        //IMapper _mapper;

//        WebAPI.DTO.LoginRequest loginRequest = new WebAPI.DTO.LoginRequest
//        {
//            Browser = "Chrome",
//            DeviceCode = "Mobile",
//            DeviceName = "Mobile",
//            IpAddress = "1.12.11.34",
//            UserName = "sysadmin",
//            Password = "Pass@123"
//        };

//        [SetUp]
//        public void Setup()
//        {
//            // setup test data
//            //mock mapper.
//            Mock<IMapper> mapper = new Mock<IMapper>();

//            //mock repository\DB calls.
//            Mock<IAuthenticationRepository> authenticationRepo = new Mock<IAuthenticationRepository>();
//            //roleRepo.Setup(mock => mock.Log()).Returns(roles);

//            // resolve dependencies.
//           // _mapper = mapper.Object;
//            //_authenticationRepository = authenticationRepo.Object;
//            //_authenticationService = new AuthenticationService(_authenticationRepository);
//        }


//        [Test]
//        public void TestGetToken()
//        {
//           // SecurityController roleController = new SecurityController(_roleService, _mapper);
//        }
//    }
//}
