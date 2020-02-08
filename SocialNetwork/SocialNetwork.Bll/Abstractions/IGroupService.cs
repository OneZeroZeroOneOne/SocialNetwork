using System.Collections.Generic;
using System.Threading.Tasks;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IGroupService
    {
        Task<List<GroupType>> GetGroupTypesAsync();
    }
}
