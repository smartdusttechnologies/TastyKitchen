namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    /// <summary>
    /// Forgot Password 
    /// </summary>
    public class ForgotPassword
    {
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// OTP
        /// </summary>
        public string otp { get; set; }
        /// <summary>
        /// New Password
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// Confirm Password
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}
