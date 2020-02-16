using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Services
{
    public class BoardService : IBoardService
    {
        private readonly PublicContext _publicContext;

        public BoardService(PublicContext publicContext)
        {
            _publicContext = publicContext;
        }

        public async Task<List<BoardType>> GetBoardTypesAsync()
        {
            return await _publicContext.BoardType.ToListAsync();
        }
    }
}
