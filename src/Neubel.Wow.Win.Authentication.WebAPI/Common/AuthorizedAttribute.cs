using Microsoft.AspNetCore.Authorization;

namespace Neubel.Wow.Win.Authentication.WebAPI.Common
{
    public class Authorized : AuthorizeAttribute
    {
        private string[] allowedRoles;
        public string[] AllowedRoles
        {
            get
            {
                return allowedRoles;
            }
            set
            {
                allowedRoles = value;
                Roles = string.Join(",", allowedRoles);
            }
        }
    }
}
