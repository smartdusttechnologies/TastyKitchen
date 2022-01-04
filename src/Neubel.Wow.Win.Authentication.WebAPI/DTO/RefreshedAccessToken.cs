using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    /// <summary>
    /// Refreshed Access Token
    /// </summary>
    public class RefreshedAccessToken
    {
        /// <summary>
        /// Access Token.
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Access Token Expiry.
        /// </summary>
        public DateTime AccessTokenExpiry { get; set; }
    }
}
