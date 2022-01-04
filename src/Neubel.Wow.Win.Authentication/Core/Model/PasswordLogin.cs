using System;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class PasswordLogin :Entity
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}