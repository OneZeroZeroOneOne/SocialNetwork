using System.Collections.Generic;
using System.Threading.Tasks;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IBoardService
    {
        Task<List<BoardType>> GetBoardTypesAsync();
        Task<List<Board>> GetBoardsAsync();

        Task<Board> GetBoardAsync(string name);
    }
}
