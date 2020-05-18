using Npgsql;
using System;

namespace SocialNetwork.Dal.Extensions
{
    public static class PostExt
    {
        public static void UpdateTop(this NpgsqlConnection conn, int postId, Guid boardId, int score)
        {
            using (var cmd = new NpgsqlCommand("updatetop", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@postId", NpgsqlTypes.NpgsqlDbType.Integer, postId);
                cmd.Parameters.AddWithValue("@boardId", NpgsqlTypes.NpgsqlDbType.Text, boardId);
                cmd.Parameters.AddWithValue("@minusScore", NpgsqlTypes.NpgsqlDbType.Integer, score);
                var ret = (bool)cmd.ExecuteScalar();
            }
        }
    }
}
