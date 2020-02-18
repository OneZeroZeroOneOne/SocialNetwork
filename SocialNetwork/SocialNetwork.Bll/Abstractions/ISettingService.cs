using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Bll.Abstractions
{
    public interface ISettingService
    {
        public Task<Setting<T>> GetSetting<T>(string key);
    }
}
