using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TastyKitchen.Web.UI.Models;

namespace TastyKitchen.Web.UI.Controllers
{
    public class SecurityController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public SecurityController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO loginRequest)
        {
            var loginReq = new LoginRequest { UserName = loginRequest.UserName, Password = loginRequest.Password };
            RequestResult<LoginToken> result = _authenticationService.Login(loginReq);

            if (result.IsSuccessful)
            {
                HttpContext.Session.SetString("Token", result.RequestedObject.AccessToken);

                return Json(new { status = true, message = "Login Successfull!" });
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult LoginOut()
        {
            HttpContext.Session.SetString("Token", null);

            return View();
        }

        //public ActionResult Validate(Admins admin)
        //{
        //    var _admin = db.Admins.Where(s => s.Email == admin.Email);
        //    if (_admin.Any())
        //    {
        //        if (_admin.Where(s => s.Password == admin.Password).Any())
        //        {

        //            return Json(new { status = true, message = "Login Successfull!" });
        //        }
        //        else
        //        {
        //            return Json(new { status = false, message = "Invalid Password!" });
        //        }
        //    }
        //    else
        //    {
        //        return Json(new { status = false, message = "Invalid Email!" });
        //    }
        //}
    }
}
