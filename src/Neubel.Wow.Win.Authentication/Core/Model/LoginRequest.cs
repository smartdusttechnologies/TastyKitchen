using System;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class LoginRequest : Entity
    {
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public string DeviceCode { get; set; }
        public string DeviceName { get; set; }
        public DateTime LoginDate { get; set; }
        public string PasswordHash { get; set; }
        public int Status { get; set; }
    }
}
