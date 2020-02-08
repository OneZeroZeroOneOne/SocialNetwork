using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services
{
    public class GroupService : IGroupService
    {
        private readonly PublicContext _publicContext;

        public GroupService(PublicContext publicContext)
        {
            _publicContext = publicContext;
        }

        public async Task<List<GroupType>> GetGroupTypesAsync()
        {
            return await _publicContext.GroupType.ToListAsync();
        }
    }
}
