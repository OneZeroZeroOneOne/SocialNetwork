using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Security.Abstractions;
using System.Threading.Tasks;


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
        public async Task<OutUserRegisterViewModel> Post(string email, string password)
        {
            var userModel = await _registrationService.Register(email, password);

            return _mapper.Map<User, OutUserRegisterViewModel>(userModel);
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<bool> Get(Guid confirmationToken)
        {
            return await _registrationService.ConfirmEmail(confirmationToken);
        }

    }
}