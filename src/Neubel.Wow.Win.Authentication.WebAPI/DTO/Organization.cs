using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    /// <summary>
    /// Organization details.
    /// </summary>
    public class Organization
    {
        public int Id { get; set; }
        /// <summary>
        /// Organization Code.
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// Organization Name.
        /// </summary>
        public string OrgName { get; set; }
    }
}
