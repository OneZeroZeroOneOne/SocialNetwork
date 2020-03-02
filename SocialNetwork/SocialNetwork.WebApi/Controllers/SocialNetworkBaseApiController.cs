using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace SocialNetwork.WebApi.Controllers
{
    public class SocialNetworkBaseApiController : ControllerBase
    {
        public static readonly Guid AnonimUser = new Guid("37f56076-4eff-400d-8ba0-0c8b37491182");

        [ApiExplorerSettings(IgnoreApi = true)]
        public Guid CurrentUser()
        {
            if (Guid.TryParse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value, out Guid currentUserId))
                return currentUserId;

            return AnonimUser;
        }
    }
}
