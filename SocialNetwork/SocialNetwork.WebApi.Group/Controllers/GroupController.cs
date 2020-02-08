using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.WebApi.Group.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<GroupType>> GetGroupTypes()
        {
            return await _groupService.GetGroupTypesAsync();
        }
    }
}
