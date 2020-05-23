using Dapper;
using SocialNetwork.Dal.Context;
using System.Data;
using System.Linq;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Dal.Dapper
{
    public static class PublicDatabase
    {
        public static int IsPostOrComment(this PublicContext context, int id)
        {
            using (IDbConnection db = context.Connection)
            {
                return db.Query<int>("SELECT ispostorcomment(@id)", new { id }).FirstOrDefault();
            }
        }

        public static Comment AddComment(this PublicContext context, Comment comment)
        {
            using (IDbConnection db = context.Connection)
            {
                return db.Query<Comment>("SELECT * from public.createcomment(@postId, @text, @userId)", new { comment.PostId, comment.Text, comment.UserId }).FirstOrDefault();
            }
        }
    }
}