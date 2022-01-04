using System;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class UserValidationOtp : Entity
    {
        public int UserId { get; set; }
        public string otp { get; set; }
        public DateTime OtpGeneratedTime { get; set; }
        public DateTime OtpAuthenticatedTime { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public int OrgId { get; set; }
    }
}
