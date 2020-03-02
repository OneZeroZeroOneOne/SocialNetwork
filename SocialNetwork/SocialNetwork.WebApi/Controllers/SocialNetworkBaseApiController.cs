using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using SocialNetwork.Utilities.Exceptions;

namespace SocialNetwork.WebApi.Controllers
{
    public class SocialNetworkBaseApiController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public Guid CurrentUser()
        {
            if (Guid.TryParse(User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value, out Guid currentUserId))
                return currentUserId;

            throw ExceptionFactory.SoftException(ExceptionEnum.UserNotFound, "User not found while getting currentUser");
        }
    }
}
