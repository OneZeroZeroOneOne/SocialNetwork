using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels.Out;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<List<OutBoardViewModel>> GetBoards()
        {
            return _mapper.Map<List<Dal.Models.Board>, List<OutBoardViewModel>>(await _groupService.GetBoardsAsync());
        }

        [HttpGet, Route("{name}", Name = "GetBoardByName")]
        public async Task<OutBoardViewModel> GetBoard([FromRoute]string name)
        {
            var board = await _groupService.GetBoardAsync(Uri.UnescapeDataString(name));
            return _mapper.Map<Dal.Models.Board, OutBoardViewModel>(board);
        }

        [HttpGet("Types")]
        public async Task<List<BoardType>> GetBoardTypes()
        {
            return await _groupService.GetBoardTypesAsync();
        }
    }
}
