using System;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class RefreshedAccessToken : Entity
    {
        /// <summary>
        /// User Id.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// User Name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Access Token.
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Access Token Expiry.
        /// </summary>
        public DateTime AccessTokenExpiry { get; set; }
        /// <summary>
        /// Device Code.
        /// </summary>
        public string DeviceCode { get; set; }
        /// <summary>
        /// Device Name.
        /// </summary>
        public string DeviceName { get; set; }
    }
}
