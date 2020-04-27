using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace SocialNetwork.Dal.Models.NoSql
{
    public class PostTop
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int PostId { get; set; }
        public int Score { get; set; }
        public Guid BoardId { get; set; }
    }
}
