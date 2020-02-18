using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using System.Threading.Tasks;
using SocialNetwork.Utilities.Exceptions;

namespace SocialNetwork.Bll.Services
{
    public class BoardService : IBoardService
    {
        private readonly PublicContext _publicContext;

        public BoardService(PublicContext publicContext)
        {
            _publicContext = publicContext;

        public async Task<List<BoardType>> GetBoardTypesAsync()
        {
            return await _publicContext.BoardType.ToListAsync();
        }

        public async Task<List<Board>> GetBoardsAsync()
        {
            return await _publicContext.Board.ToListAsync();
        }

        public async Task<Board> GetBoardAsync(string name)
        {
            var board = await _publicContext.Board.FirstOrDefaultAsync(x => x.Name == name);

            return board;
        }
    }
}
