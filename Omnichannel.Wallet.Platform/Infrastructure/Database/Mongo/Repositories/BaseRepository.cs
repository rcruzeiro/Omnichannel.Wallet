using System;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Infrastructure.Database.Mongo.Repositories
{
    public abstract class BaseRepository<T>
        where T : class
    {
        readonly string _collection;
        readonly IMongoDatabase _database;

        protected IMongoCollection<T> Collection
        { get { return _database.GetCollection<T>(_collection); } }

        protected BaseRepository(string connstring, string database, string collection)
        {
            _collection = collection;
            RegisterDiscriminators();
            MongoClient client = new MongoClient(connstring);
            _database = client.GetDatabase(database);
        }

        void RegisterDiscriminators()
        {
            try
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(Transaction)))
                    BsonClassMap.RegisterClassMap<Transaction>(bcm =>
                    {
                        bcm.AutoMap();
                        bcm.SetIdMember(bcm.GetMemberMap(c => c.ID));

                        // removes the circular reference from the serializer
                        bcm.UnmapMember(c => c.Account);
                    });
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
