namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class ForgotPassword
    {
        public string UserName { get; set; }
        public string otp { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
