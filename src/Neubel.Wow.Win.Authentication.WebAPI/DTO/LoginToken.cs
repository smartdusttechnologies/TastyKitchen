using System;

namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    /// <summary>
    /// User token details.
    /// </summary>
    public class LoginToken
    {
        /// <summary>
        /// Access Token.
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Access Token Expiry.
        /// </summary>
        public DateTime AccessTokenExpiry { get; set; }
        /// <summary>
        /// Refresh Token.
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// Refresh Token Expiry.
        /// </summary>
        public DateTime RefreshTokenExpiry { get; set; }
    }
}