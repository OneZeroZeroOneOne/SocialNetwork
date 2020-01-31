using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Security.Abstractions;


namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IMapper _mapper;

        private readonly IRegistrationService _registrationService;
        public RegistrationController(ILogger<RegistrationController> logger,IMapper mapper, IRegistrationService registrationService)
        {
            _logger = logger;
            _mapper = mapper;
            _registrationService = registrationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string email, string password)
        {
            var userModel = await _registrationService.Register(email, password);

            var userOutViewModel = _mapper.Map<User, OutUserRegisterViewModel>(userModel);

            return Ok(userOutViewModel);
        }

    }
}