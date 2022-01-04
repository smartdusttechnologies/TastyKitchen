using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    /// <summary>
    /// Roles details.
    /// </summary>
    public class Role
    {
        public int Id { get; set; }
        /// <summary>
        /// Role Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tole level.
        /// </summary>
        public int Level { get; set; }
    }
}
