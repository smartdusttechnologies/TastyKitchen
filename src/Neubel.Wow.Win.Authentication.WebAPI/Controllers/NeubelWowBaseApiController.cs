using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication.Common;
using System.Linq;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    public abstract class NeubelWowBaseApiController : ControllerBase
    {
        public SessionContext SessionContext
        {
            get
            {
                string authorization = HttpContext.Request.Headers["Authorization"].SingleOrDefault();
                return new SessionContext
                {
                    Authorization = authorization,
                    OrganizationId = string.IsNullOrEmpty(authorization)? null : Helpers.GetOrganizationContextForSignedInUser(authorization)
                };
            }
        }
    }
}
