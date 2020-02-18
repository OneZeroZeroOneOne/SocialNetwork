using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Dal.Context;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GlobalSettingsController : SocialNetworkBaseApiController
    {
        private readonly ILogger<GlobalSettingsController> _logger;
        private readonly ISettingService _settingService;

        public GlobalSettingsController(ILogger<GlobalSettingsController> logger, ISettingService settingService)
        {
            _logger = logger;
            _settingService = settingService;
        }

        [HttpGet, Route("n/{key}", Name = "numberSettingByKey")]
        public async Task<Setting<int>> GetNumberSetting([FromRoute]string key)
        {
            return await _settingService.GetSetting<int>(key);
        }

        [HttpGet, Route("s/{key}", Name = "stringSettingByKey")]
        public async Task<Setting<string>> GetStringSetting([FromRoute]string key)
        {
            return await _settingService.GetSetting<string>(key);
        }
    }
}