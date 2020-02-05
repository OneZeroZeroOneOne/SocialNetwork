﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetwork.WebApi.Controllers
{
    public class SocialNetworkBaseApiController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<User> CurrentUser()
        {
            PublicContext publicContext = new PublicContext();
            if (Guid.TryParse(this.User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value, out Guid currentUserId))
                return await publicContext.User.FirstOrDefaultAsync(x => x.Id == currentUserId);

            return null;//??FUCK??//
        }
    }
}