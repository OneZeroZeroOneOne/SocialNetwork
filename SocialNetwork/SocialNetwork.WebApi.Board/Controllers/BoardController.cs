using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.WebApi.Board.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController
    {
        private readonly IBoardService _groupService;
        private readonly IMapper _mapper;

        public BoardController(IBoardService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<BoardType>> GetGroupTypes()
        {
            return await _groupService.GetBoardTypesAsync();
        }
    }
}
