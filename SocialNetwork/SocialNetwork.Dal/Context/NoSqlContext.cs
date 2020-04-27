using MongoDB.Driver;
using SocialNetwork.Dal.Models.NoSql;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Context
{
    public class NoSqlContext
    {
        private readonly string _connectionString;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        private readonly Dictionary<Type, string> _collectionName;

        public NoSqlContext(string connectionString)
        {
            _connectionString = connectionString;

            _client = new MongoClient(_connectionString);
            _database = _client.GetDatabase("SocialNetwork");
            _collectionName = new Dictionary<Type, string>()
            {
                {typeof(PostTop), "PostPaging" },
            };
        }

        public IMongoCollection<T> Collection<T>()
        {
            return _database.GetCollection<T>(_collectionName[typeof(T)]);
        }
    }
}
