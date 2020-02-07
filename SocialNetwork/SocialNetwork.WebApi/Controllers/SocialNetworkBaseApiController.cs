using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SocialNetwork.Utilities.Abstractions;
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
