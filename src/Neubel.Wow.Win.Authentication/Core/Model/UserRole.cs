using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class UserRole : Entity
    {
        /// <summary>
        /// User Name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// User Name.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        public int RoleId { get; set; }
    }
}
