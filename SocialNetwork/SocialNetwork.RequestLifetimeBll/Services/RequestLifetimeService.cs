using SocialNetwork.Dal.Models;
using SocialNetwork.RequestLifetimeBll.Abstractions;

namespace SocialNetwork.RequestLifetimeBll.Services
{
    public class RequestLifetimeService : IRequestLifetimeService
    {
        public Board Board { get; set; }
        public Post Post { get; set; }

        public void SetBoard(Board board)
        {
            Board = board;
        }

        public void SetPost(Post post)
        {
            Post = post;
        }
    }
}
