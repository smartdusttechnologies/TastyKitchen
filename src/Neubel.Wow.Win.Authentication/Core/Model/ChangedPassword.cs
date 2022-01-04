namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class ChangedPassword
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
