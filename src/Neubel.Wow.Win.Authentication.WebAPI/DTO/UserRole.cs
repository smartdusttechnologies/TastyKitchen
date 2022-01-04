using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    public class UserRole
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
