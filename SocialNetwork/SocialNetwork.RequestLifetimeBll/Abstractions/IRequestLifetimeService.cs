using SocialNetwork.Dal.Models;

namespace SocialNetwork.RequestLifetimeBll.Abstractions
{
    public interface IRequestLifetimeService
    {
        void SetBoard(Board board);
        void SetPost(Post post);
    }
}
