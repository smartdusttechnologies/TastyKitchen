namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    /// <summary>
    /// Login request details.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// IpAddress
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// Browser
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// Device Code
        /// </summary>
        public string DeviceCode { get; set; }
        /// <summary>
        /// Device Name
        /// </summary>
        public string DeviceName { get; set; }
    }
}