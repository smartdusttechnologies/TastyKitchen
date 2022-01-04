using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    /// <summary>
    /// Change password details.
    /// </summary>
    public class ChangedPassword
    {
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Current Password
        /// </summary>
        public string CurrentPassword { get; set; }
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
