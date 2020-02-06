using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Dal.ViewModels.Out;
using SocialNetwork.Security.Abstractions;
using System.Threading.Tasks;

namespace SocialNetwork.WebApi.AuthorizationServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IAuthenticationService _authService;
        public AuthorizationController(ILogger<AuthorizationController> logger, IAuthenticationService authService)
        {
            _logger = logger;

            _authService = authService;
        }

        [HttpGet]
        public async Task<OutRegistrationViewModel> Get(string email, string password)
        {
            var identity = await _authService.GetIdentity(email, password);

            var encodedJwt = _authService.GenerateToken(identity);

            return new OutRegistrationViewModel
            {
                access_token = encodedJwt,
                email = email,
            };
        }
    }
}