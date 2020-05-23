using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Dal.Models;
using SocialNetwork.RequestLifetimeBll.Abstractions;

namespace SocialNetwork.RequestLifetimeBll.Services
{
    public class RequestLifetimeService : IRequestLifetimeService
    {
        public Board Board { get; set; }
        public Post Post { get; set; }

        public List<Mention> Mentions { get; set; } = new List<Mention>();

        public int MyId { get; set; }
        public bool IsComment { get; set; }

        public void SetBoard(Board board)
        {
            Board = board;
        }

        public void SetPost(Post post)
        {
            Post = post;
        }

        public void AddMention(Mention mention)
        {
            if (Mentions == null)
                Mentions = new List<Mention>();

            if (Mentions.FirstOrDefault(x => x.MentionerId == mention.MentionerId) == null)
                Mentions.Add(mention);
        }

        public void SetMyId(int id, bool isComment)
        {
            MyId = id;
            IsComment = isComment;
        }

        public List<Mention> GetMentions()
        {
            return Mentions;
        }
    }
}
