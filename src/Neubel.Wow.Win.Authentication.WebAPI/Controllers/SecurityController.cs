using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model.Roles;
using Neubel.Wow.Win.Authentication.WebAPI.Common;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : NeubelWowBaseApiController
    {
        private readonly Core.Interfaces.IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public SecurityController(Core.Interfaces.IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Get the access and refresh token after authentication.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [Route("token")]
        [HttpPost]
        public IActionResult Token(DTO.LoginRequest loginRequest)
        {
            var loginReq = _mapper.Map<DTO.LoginRequest, Core.Model.LoginRequest>(loginRequest);
            var result = _authenticationService.Login(loginReq);

            if (result.IsSuccessful)
            {
                DTO.LoginToken userToken = _mapper.Map<Core.Model.LoginToken, DTO.LoginToken>(result.RequestedObject);
               // HttpContext.Session.SetString("JWToken", userToken.AccessToken);
                return Ok(userToken);
            }

            return Ok(result.ValidationMessages);
        }
        /// <summary>
        /// Refresh a access token using refresh token.
        /// </summary>
        /// <returns></returns>
        [Route("refreshToken")]
        [Authorize]
        [HttpPut]
        public IActionResult RefreshToken()
        {
            string authorization = HttpContext.Request.Headers["Authorization"].SingleOrDefault();

            Core.Model.RefreshedAccessToken refreshedAccessToken = _authenticationService.RefreshToken(authorization);
            if (refreshedAccessToken == null)
                return Unauthorized();

            var userToken = _mapper.Map<Core.Model.RefreshedAccessToken, DTO.RefreshedAccessToken>(refreshedAccessToken);

            return Ok(userToken);
        }

        /// <summary>
        /// Forget password.
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns></returns>
        [Route("forgotPassword")]
        [HttpPost]
        public IActionResult ForgotPassword(Core.Model.ForgotPassword forgotPassword)
        {
           var result = _authenticationService.ForgotPassword(forgotPassword);

            if (result.IsSuccessful)
                return Ok(result.RequestedObject);

            return Ok(result.ValidationMessages);
        }
        /// <summary>
        /// Change passsword.
        /// </summary>
        /// <param name="changedPassword"></param>
        /// <returns></returns>
        //[Authorize]
        [Route("changePassword")]
        [HttpPost]
        public IActionResult ChangePassword(DTO.ChangedPassword changedPassword)
        {
            var newPassword = _mapper.Map<DTO.ChangedPassword, Core.Model.ChangedPassword>(changedPassword);
            var result = _authenticationService.ChangePassword(SessionContext, newPassword);

            if (result.IsSuccessful)
                return Ok(result.RequestedObject);

            return Ok(result.ValidationMessages);
        }
        /// <summary>
        /// Send Otp.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Route("sendOtp")]
        [HttpPost]
        public IActionResult SendOTP(string userName)
        {
            bool result = _authenticationService.SendOtp(userName);
            return Ok(result);
        }
        /// <summary>
        /// Validate or match otp.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        [Route("validateOtp")]
        [HttpPost]
        public IActionResult ValidateOtp(string userName, string otp)
        {
            bool result = _authenticationService.ValidateOtp(userName, otp);
            return Ok(result);
        }
        /// <summary>
        /// Confirm Mobile.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        [Route("confirmMobile")]
        [HttpPost]
        [Authorize]
        public IActionResult ConfirmMobile(string userName, string otp)
        {
            bool result = _authenticationService.UpdateMobileConfirmationStatus(userName, otp);
            return Ok(result);
        }
        /// <summary>
        /// Confirm email.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        [Route("confirmEmail")]
        [HttpPost]
        [Authorize]
        public IActionResult ConfirmEmail(string userName, string otp)
        {
            bool result = _authenticationService.UpdateEmailConfirmationStatus(userName, otp);
            return Ok(result);
        }
        /// <summary>
        /// Login history.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("loginHistory")]
        [HttpGet]
        [Authorized(AllowedRoles = new[] { UserRoles.ApplicationAdmin, UserRoles.Admin })]
        public IActionResult LoginHistory(int userId)
        {
            return Ok(_authenticationService.GetLoginHistory(SessionContext, userId));
        }
        /// <summary>
        /// Lock user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Route("lock")]
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Lock(string userName)
        {
            bool result = _authenticationService.LockUnlockUser(SessionContext, new Core.Model.LockUnlockUser { UserName = userName, Locked = true });
            return Ok(result);
        }
        /// <summary>
        /// Unlock user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Route("unLock")]
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult UnLock(string userName)
        {
            bool result = _authenticationService.LockUnlockUser(SessionContext, new Core.Model.LockUnlockUser { UserName = userName, Locked = false});
            return Ok(result);
        }
    }
}