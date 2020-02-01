using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using SocialNetwork.Bll.Services;
using SocialNetwork.Bll.Abstractions;


namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private IAuthenticationService _authService;
        public AuthorizationController(ILogger<AuthorizationController> logger, IAuthenticationService authService)
        {
            _logger = logger;
            _context = new PublicContext();
            _authService = authService;

        }

        [HttpGet]
        public IActionResult Get(string email, string password)
        {
            var identity = _authService.GetIdentity(email, password);

            var encodedJwt = _authService.GenerateToken(identity);

            var response = new
            {
                access_token = encodedJwt,
                email,
            };

            return Ok(response);
        }
        
    }
}