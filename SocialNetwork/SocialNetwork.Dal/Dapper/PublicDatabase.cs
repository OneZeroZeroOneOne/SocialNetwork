using Dapper;
using SocialNetwork.Dal.Context;
using System.Data;
using System.Linq;

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
    }
}