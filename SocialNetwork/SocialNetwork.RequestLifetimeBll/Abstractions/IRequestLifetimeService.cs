using System.Collections.Generic;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.RequestLifetimeBll.Abstractions
{
    public interface IRequestLifetimeService
    {
        void SetBoard(Board board);
        void SetPost(Post post);
        void AddMention(Mention mention);
        void SetMyId(int id, bool isComment);
        List<Mention> GetMentions();
    }
}
